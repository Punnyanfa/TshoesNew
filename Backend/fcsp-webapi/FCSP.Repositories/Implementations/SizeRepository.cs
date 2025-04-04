using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(FcspDbContext context) : base(context)
        {
        }
        
        public async Task<Size> GetSizeEntityBySizeValueAsync(int sizeValue)
        {
            return await Entities.FirstOrDefaultAsync(s => s.SizeValue == sizeValue);
        }
        // Implement any specific methods for Size if needed
    }
} 