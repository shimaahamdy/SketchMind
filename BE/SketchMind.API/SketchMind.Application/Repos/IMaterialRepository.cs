using SketchMind.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Repos
{
    public interface IMaterialRepository
    {
        Task<Material?> GetByIdAsync(int materialId,CancellationToken cancellationToken);
    }
}
