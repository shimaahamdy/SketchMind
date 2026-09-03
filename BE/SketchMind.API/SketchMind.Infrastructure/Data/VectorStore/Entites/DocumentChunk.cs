using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data.VectorStore.Entites
{
    public class DocumentChunk
    {
        public Guid Id { get; set; }
        public Guid UsertId { get; set; }
        public int ChunkIndex { get; set; }
        public Guid MaterialId { get; set; }

        public string Text { get; set; } = string.Empty;

        public int? PageNumber { get; set; }

        public float[] Embedding { get; set; } = [];

        public DateTime CreatedAt { get; set; }

    }
}
