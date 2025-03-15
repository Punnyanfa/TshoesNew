using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class ReturnedCustomShoeRepository : GenericRepository<ReturnedCustomShoe>, IReturnedCustomShoeRepository
    {
        public ReturnedCustomShoeRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReturnedCustomShoe>> GetByCustomShoeDesignIdAsync(long customShoeDesignId)
        {
            return await Entities
                .Where(r => r.CustomShoeDesignId == customShoeDesignId && !r.IsDeleted)
                .Include(r => r.CustomShoeDesign)
                .ToListAsync();
        }
    }
}