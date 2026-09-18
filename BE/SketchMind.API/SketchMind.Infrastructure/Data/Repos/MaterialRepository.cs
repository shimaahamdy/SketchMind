using Microsoft.EntityFrameworkCore;
using SketchMind.Application.Repos;
using SketchMind.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data.Repos
{
    public sealed class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Material?> GetByIdAsync(int materialId, CancellationToken cancellationToken)
        {
           var material = _context.Materials.FirstOrDefaultAsync(materal => materal.Id == materialId,cancellationToken);
            return material;
     
        }
    }
}
