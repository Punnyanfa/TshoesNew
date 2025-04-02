using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(FcspDbContext context) : base(context)
        {
        }
        
        // Implement any specific methods for Size if needed
    }
} 