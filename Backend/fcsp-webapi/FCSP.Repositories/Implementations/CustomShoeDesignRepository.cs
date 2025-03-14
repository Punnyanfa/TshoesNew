using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignRepository : GenericRepository<CustomShoeDesign>, ICustomShoeDesignRepository
    {
        public CustomShoeDesignRepository(FcspDbContext context) : base(context)
        {
        }
        
        public async Task<IList<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId)
        {
            return await Entities
                .Where(d => d.UserId == userId)
                .Include(d => d.CustomShoeDesignTextures)
                .ToListAsync();
        }
    }
}