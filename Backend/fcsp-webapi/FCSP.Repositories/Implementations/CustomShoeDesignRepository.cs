using Azure.Core;
using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignRepository : GenericRepository<CustomShoeDesign>, ICustomShoeDesignRepository
    {
        public CustomShoeDesignRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CustomShoeDesign>> GetAllPublicCustomShoeDesignsAsync()
        {
            return await Entities
                .Include(d => d.Ratings)
                .Include(d => d.User)
                .Include(d => d.CustomShoeDesignTemplate)
                .Include(d => d.CustomShoeDesignTextures)
                    .ThenInclude(t => t.Texture)
                .Include(d => d.DesignPreviews)
                .Include(d => d.DesignServices)
                .Where(d => d.IsDeleted == false)
                .Where(d => d.Status == Common.Enums.CustomShoeDesignStatus.Public)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId)
        {
            return await Entities
                .Where(d => d.UserId == userId)
                .Include(d => d.Ratings)
                .Include(d => d.User)
                .Include(d => d.CustomShoeDesignTemplate)
                .Include(d => d.CustomShoeDesignTextures)
                    .ThenInclude(t => t.Texture)
                .Include(d => d.DesignPreviews)
                .Include(d => d.DesignServices)
                .Where(d => d.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomShoeDesign>> GetDesignsByIdsAsync(IEnumerable<long> designIds)
        {
            return await Entities
                .Where(d => designIds.Contains(d.Id))
                .Include(d => d.Ratings)
                .Include(d => d.User)
                .Include(d => d.CustomShoeDesignTemplate)
                .Include(d => d.CustomShoeDesignTextures)
                    .ThenInclude(t => t.Texture)
                .Include(d => d.DesignPreviews)
                .Include(d => d.DesignServices)
                .Where(d => d.IsDeleted == false)
                .ToListAsync();
        }
        public async Task<int> GetTotalAmountByIdAsync(long id)
        {
            var design = await Entities
                .Where(d => d.Id == id && !d.IsDeleted)
                .Select(d => d.TotalAmount)
                .FirstOrDefaultAsync();

            return design; 
        }

    }
}