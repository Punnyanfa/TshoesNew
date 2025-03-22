using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignTextureRepository : GenericRepository<CustomShoeDesignTexture>, ICustomShoeDesignTextureRepository
    {
        private readonly FcspDbContext _dbContext;

        public CustomShoeDesignTextureRepository(FcspDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task AddRangeAsync(IEnumerable<CustomShoeDesignTexture> entities)
        {
            await _dbContext.CustomShoeDesignTextures.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<long> designIds)
        {
            var texturesToDelete = await _dbContext.CustomShoeDesignTextures.Where(t => designIds.Contains(t.CustomShoeDesignId)).ToListAsync();
            _dbContext.CustomShoeDesignTextures.RemoveRange(texturesToDelete);
            await _dbContext.SaveChangesAsync();
        }

        // Implement any custom repository methods here
    }
}