using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignTexturesRepository : GenericRepository<CustomShoeDesignTextures>, ICustomShoeDesignTexturesRepository
    {
        private readonly FcspDbContext _dbContext;

        public CustomShoeDesignTexturesRepository(FcspDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task AddRangeAsync(IEnumerable<CustomShoeDesignTextures> entities)
        {
            await _dbContext.CustomShoeDesignTextures.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<long> textureIds)
        {
            var texturesToDelete = await _dbContext.CustomShoeDesignTextures.Where(t => textureIds.Contains(t.TextureId)).ToListAsync();
            _dbContext.CustomShoeDesignTextures.RemoveRange(texturesToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomShoeDesignTextures>> GetByCustomShoeDesignIdAsync(long customShoeDesignId)
        {
            return await Entities
                .Where(i => i.CustomShoeDesignId == customShoeDesignId)
                .Include(i => i.Texture)
                .ToListAsync();
        }
    }
}