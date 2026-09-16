using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data.VectorStore.Mongo
{
    // one mongo Document will represent one chunk
    public class MongoDocumentChunk
    {
       
        public int ID { get; set; }
        public int UsertId { get; set; }
        public int ChunkIndex { get; set; }
        public int MaterialId { get; set; }

        public string Text { get; set; } = string.Empty;

        public int? PageNumber { get; set; }

        public float[] Embedding { get; set; } = [];

        public DateTime CreatedAt { get; set; }

    }
}
