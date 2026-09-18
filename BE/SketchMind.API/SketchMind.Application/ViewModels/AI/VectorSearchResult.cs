using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.ViewModels.AI
{
    public sealed class VectorSearchResult
    {
        public string ChunkId { get; init; } = string.Empty;

        public int MaterialId { get; init; }

        public string Text { get; init; } = string.Empty;

        public int? PageNumber { get; init; }

        public double Similarity { get; init; }
    }
}
