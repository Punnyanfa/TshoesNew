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

        public async Task AddRangeAsync(IEnumerable<DesignPreview> previews)
        {
            await Entities.AddRangeAsync(previews);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<long> previewIds)
        {
            var designPreviewsToDelete = await _context.DesignPreviews.Where(dp => previewIds.Contains(dp.Id)).ToListAsync();
            _context.DesignPreviews.RemoveRange(designPreviewsToDelete);
            await _context.SaveChangesAsync();
        }
    }
}