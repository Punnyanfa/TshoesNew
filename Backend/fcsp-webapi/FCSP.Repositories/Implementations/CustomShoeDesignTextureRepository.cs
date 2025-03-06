using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignTextureRepository : GenericRepository<CustomShoeDesignTexture>, ICustomShoeDesignTextureRepository
    {
        private readonly FcspDbContext _context;
        
        public CustomShoeDesignTextureRepository(FcspDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task AddRangeAsync(IEnumerable<CustomShoeDesignTexture> entities)
        {
            await _context.CustomShoeDesignTextures.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteByDesignIdAsync(long designId)
        {
            var texturesToDelete = await _context.CustomShoeDesignTextures
                .Where(t => t.CustomShoeDesignId == designId)
                .ToListAsync();
                
            if (texturesToDelete.Any())
            {
                _context.CustomShoeDesignTextures.RemoveRange(texturesToDelete);
                await _context.SaveChangesAsync();
            }
        }

        // Implement any custom repository methods here
    }
}