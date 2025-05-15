using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class DesignPreviewRepository : GenericRepository<DesignPreview>, IDesignPreviewRepository
    {
        public DesignPreviewRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DesignPreview>> GetPreviewsByCustomShoeDesignIdAsync(long customShoeDesignId)
        {
            return await Entities
                .Include(dp => dp.CustomShoeDesign)
                .Where(dp => dp.CustomShoeDesignId == customShoeDesignId)
                .OrderByDescending(dp => dp.CreatedAt)
                .ToListAsync();
        }
    }
}