using SketchMind.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Interfaces.AI
{
    public interface IEmbeddingGenerator
    {
        Task<IReadOnlyList<EmbeddingResult>> GenerateAsync(IReadOnlyList<string> texts, CancellationToken cancellationToken);

    }
}
