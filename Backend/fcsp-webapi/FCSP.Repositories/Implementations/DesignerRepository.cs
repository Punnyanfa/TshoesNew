using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class DesignerRepository : GenericRepository<Designer>, IDesignerRepository
    {
        public DesignerRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<Designer?> GetDesignerByUserIdAsync(long userId)
        {
            return await Entities
                .Where(d => d.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Designer>> GetActiveDesignersAsync()
        {
            return await Entities
                .Where(d => d.Status > 0) // Assuming positive status means active
                .Include(d => d.User)
                .ToListAsync();
        }
    }
}