using SketchMind.Application.ViewModels.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.AI
{
    public interface IEmbeddingGenerator
    {
        Task<IReadOnlyList<EmbeddingResult>> GenerateAsync(IReadOnlyList<string> texts, CancellationToken cancellationToken);

    }
}
