using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts
{
    public interface IMaterialIngestionService
    {
        Task ProcessAsync(int materialId,CancellationToken cancellationToken);
    }
}
