using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class TemplateRepository : GenericRepository<CustomShoeDesignTemplate>, ICustomShoeDesignTemplateRepository
    {
        private readonly FcspDbContext _context;

        public TemplateRepository(FcspDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<long>> GetCustomShoeDesignIdsByTemplateAsync(long templateId)
        {
            return await _context.CustomShoeDesigns
                .Where(d => d.CustomShoeDesignTemplateId== templateId && !d.IsDeleted) 
                .Select(d => d.Id)
                .ToListAsync();
        }

        public async Task<int> GetCustomShoeDesignCountByTemplateAsync(long templateId)
        {
            return await _context.CustomShoeDesigns
                .CountAsync(d => d.CustomShoeDesignTemplateId == templateId && !d.IsDeleted); 
        }
    }
}