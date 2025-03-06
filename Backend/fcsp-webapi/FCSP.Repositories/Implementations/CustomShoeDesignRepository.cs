using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignRepository : GenericRepository<Models.Entities.CustomShoeDesign>, ICustomShoeDesignRepository
    {
        private readonly FcspDbContext _context;
        
        public CustomShoeDesignRepository(FcspDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IList<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId)
        {
            return await _context.CustomShoeDesigns
                .Where(d => d.UserId == userId)
                .Include(d => d.CustomShoeDesignTextures)
                .ToListAsync();
        }
    }
}