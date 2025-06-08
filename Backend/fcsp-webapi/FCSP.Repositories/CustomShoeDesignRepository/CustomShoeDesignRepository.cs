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
                    .ThenInclude(d => d.Service)
                .Where(d => d.IsDeleted == false && d.Status == Common.Enums.CustomShoeDesignStatus.Public)
                .OrderByDescending(d => d.CreatedAt)
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
                    .ThenInclude(d => d.Service)
                .Where(d => d.IsDeleted == false)
                .OrderByDescending(d => d.CreatedAt)
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
                    .ThenInclude(d => d.Service)
                .Where(d => d.IsDeleted == false)
                .ToListAsync();
        }
    }
}