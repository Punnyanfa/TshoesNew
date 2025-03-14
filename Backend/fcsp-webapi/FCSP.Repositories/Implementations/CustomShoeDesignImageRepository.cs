using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignImageRepository : GenericRepository<CustomShoeDesignImage>, ICustomShoeDesignImageRepository
    {
        public CustomShoeDesignImageRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CustomShoeDesignImage>> GetByCustomShoeDesignIdAsync(long customShoeDesignId)
        {
            return await Entities
                .Where(i => i.CustomShoeDesignId == customShoeDesignId)
                .Include(i => i.Texture)
                .ToListAsync();
        }
    }
} 