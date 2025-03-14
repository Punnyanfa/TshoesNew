using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(FcspDbContext context) : base(context)
        {
        }
        
        public async Task<IList<Service>> GetByManufacturerIdAsync(long manufacturerId)
        {
            return await Entities
                .Where(s => s.ManufacturerId == manufacturerId && !s.IsDeleted)
                .Include(s => s.Manufacturer)
                .Include(s => s.SetServiceAmounts)
                .ToListAsync();
        }
        
        public async Task<IList<Service>> GetActiveServicesAsync()
        {
            return await Entities
                .Where(s => !s.IsDeleted)
                .Include(s => s.Manufacturer)
                .Include(s => s.SetServiceAmounts)
                .ToListAsync();
        }
    }
} 