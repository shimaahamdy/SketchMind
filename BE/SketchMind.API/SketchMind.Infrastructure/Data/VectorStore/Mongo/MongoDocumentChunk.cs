using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data.VectorStore.Mongo
{
    // one mongo Document will represent one chunk
    public class MongoDocumentChunk
    {
        public string ID { get; set; } = string.Empty;
        public int UserId { get; set; }

        public int MaterialId { get; set; }

        public int ChunkIndex { get; set; }

        public string Text { get; set; } = string.Empty;

        public int? PageNumber { get; set; }

        public float[] Embedding { get; set; } = [];

        public Dictionary<string, object>? Metadata { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
