using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.AI.Embedding.DTOs
{
    internal sealed class EmbeddingResponse
    {
        public List<EmbeddingData> Data { get; set; } = [];
    }

    internal sealed class EmbeddingData
    {
        public int Index { get; set; }
        public float[] Embedding { get; set; } = [];
    }
}
