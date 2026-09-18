using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SketchMind.Application.Contracts.AI;
using SketchMind.Application.ViewModels.AI;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SketchMind.Infrastructure.Data.VectorStore.Mongo
{
    internal class MongoVectorStore : IVectorStore
    {
        private readonly IMongoCollection<MongoDocumentChunk> _collection;

        // get collecction data from Database
        public MongoVectorStore(IMongoDatabase database,IOptions<MongoDbOptions> options)
        {
            _collection = database.GetCollection<MongoDocumentChunk>(
                options.Value.CollectionName);
        }



        internal void ConfigureIndexes()
        {
            var index = new CreateIndexModel<MongoDocumentChunk>(
                Builders<MongoDocumentChunk>.IndexKeys
                .Ascending(x => x.MaterialId));

            _collection.Indexes.CreateOne(index);
        }

        private static MongoDocumentChunk MapToDocument(VectorDocument source)
        {
            return new MongoDocumentChunk
            {
                ID = $"{source.MaterialId}_{source.ChunkIndex}",

                UserId = source.UserId,
                MaterialId = source.MaterialId,
                ChunkIndex = source.ChunkIndex,
                Text = source.Text,
                PageNumber = source.PageNumber,
                Embedding = source.Embedding,

                Metadata = source.Metadata?
            .ToDictionary(x => x.Key, x => x.Value),

                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task InsertAsync(IReadOnlyList<VectorDocument> documents, CancellationToken cancellationToken)
        {
            if (documents.Count == 0)
                return;

            var operations = documents.Select(document =>
            {
                var mongoDocument = MapToDocument(document);

                // insert new document chunk in monog or replace it if it exist
                // using mongoDocumentID as ID used to filter document chunks
                return new ReplaceOneModel<MongoDocumentChunk>(
                    Builders<MongoDocumentChunk>
                    .Filter
                    .Eq(x => x.ID, mongoDocument.ID),
                    mongoDocument)
                {
                    IsUpsert = true
                };


            }).ToList();

            // insert in mongo bulk operation all the doucments
            await _collection.BulkWriteAsync(
              operations,
              cancellationToken: cancellationToken);



        }

        public async Task<IReadOnlyList<VectorSearchResult>> SearchAsync(VectorSearchQuery query,
            CancellationToken cancellationToken)
        {

            // create victor search similarty using specific index
            var vectorSearchStage = new BsonDocument("$vectorSearch",
                new BsonDocument
                {
                    { "index", "document_chunks_vector_index" },  // index name
                    { "path", "embedding" },  // Search against the embedding field.
                    { "queryVector", new BsonArray(query.QueryVector) },  // value used to search
                    { "numCandidates", query.TopK * 10 },  // # documents to search within
                    { "limit", query.TopK },    // result 
                    {
                        "filter",                      // define filter using userID
                        new BsonDocument(
                            "userId",
                            query.UserId)
                    }
                });

            // result will be returned 
            var projectStage = new BsonDocument("$project",
                new BsonDocument
                {
                    { "_id", 1 },
                    { "materialId", 1 },
                    { "text", 1 },
                    { "pageNumber", 1 },
                    {
                        "similarity",
                        new BsonDocument(
                            "$meta",
                            "vectorSearchScore")
                    }
                });

            // phased of search and reterival configuration
            // MongoDB Aggregation Pipeline
            var pipeline = new[]
            {
                vectorSearchStage,
                projectStage
            };

            // Execute this aggregation pipeline against the collection

            var results = await _collection
                .Aggregate<MongoDocumentChunk>(pipeline)
                .ToListAsync(cancellationToken);

            return results
                .Select(x => new VectorSearchResult
                {
                    ChunkId = x.ID,
                    MaterialId = x.MaterialId,
                    Text = x.Text,
                    PageNumber = x.PageNumber
                })
                .ToList();
        }

        public async Task DeleteByMaterialIdAsync(int materialId, CancellationToken cancellationToken)
        {
            var filter = Builders<MongoDocumentChunk>
                .Filter
                .Eq(x => x.MaterialId, materialId);

            await _collection.DeleteManyAsync(
                filter,
                cancellationToken);
        }













    }
}
