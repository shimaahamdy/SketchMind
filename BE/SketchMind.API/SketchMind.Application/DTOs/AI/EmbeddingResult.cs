using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.DTOs.AI
{
    public sealed class EmbeddingResult
    {
        public float[] Vector { get; init; } = [];
    }
}
