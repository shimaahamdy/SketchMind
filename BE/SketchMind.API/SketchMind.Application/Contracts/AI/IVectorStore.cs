using SketchMind.Application.ViewModels.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.AI
{
    public interface IVectorStore
    {

        Task InsertAsync(IReadOnlyList<VectorDocument> documents, CancellationToken cancellationToken);

    }
}
