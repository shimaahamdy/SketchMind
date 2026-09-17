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

        public MongoVectorStore(IMongoDatabase database)
        {
            _collection = database.GetCollection<MongoDocumentChunk>(
                "DocumentChunks");
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
                ID = source.Id,
                UserId = source.UserId,
                MaterialId = source.MaterialId,
                ChunkIndex = source.ChunkIndex,
                Text = source.Text,
                PageNumber = source.PageNumber,
                Embedding = source.Embedding,
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













    }
}
