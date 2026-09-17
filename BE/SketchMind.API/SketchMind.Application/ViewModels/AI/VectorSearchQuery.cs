using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.ViewModels.AI
{
    public sealed class VectorSearchQuery
    {
        public int UserId { get; init; }

        public float[] QueryVector { get; init; } = [];

        public int TopK { get; init; }

        public IReadOnlyCollection<Guid>? MaterialIds { get; init; }
    }
}
