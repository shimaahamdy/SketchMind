using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.ViewModels
{
    public sealed class TextChunk
    {
        public int ChunkIndex { get; init; }

        public string Text { get; init; } = string.Empty;

        public int? PageNumber { get; init; }

        public IReadOnlyDictionary<string, object>? Metadata { get; init; }
    }
}
