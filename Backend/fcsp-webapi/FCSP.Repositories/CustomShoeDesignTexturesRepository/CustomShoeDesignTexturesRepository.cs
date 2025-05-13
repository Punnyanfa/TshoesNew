using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignTexturesRepository : GenericRepository<CustomShoeDesignTextures>, ICustomShoeDesignTexturesRepository
    {
        public CustomShoeDesignTexturesRepository(FcspDbContext context) : base(context)
        {
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