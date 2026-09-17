using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.ViewModels.AI
{
    public class VectorDocument
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int MaterialId { get; init; }
        public int ChunkIndex { get; init; }
        public string Text { get; init; } = string.Empty;
        public int? PageNumber { get; init; }
        public float[] Embedding { get; init; } = [];
        public IReadOnlyDictionary<string, object>? Metadata { get; init; }
    }
}
