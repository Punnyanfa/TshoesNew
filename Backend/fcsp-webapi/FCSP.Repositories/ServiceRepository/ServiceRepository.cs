using FCSP.Common.Enums;
using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IList<Service>> GetActiveServicesAsync()
        {
            return await Entities
                .Where(s => !s.IsDeleted && s.Manufacturer.Status == ManufacturerStatus.Active)
                .Include(s => s.Manufacturer)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Service> GetServiceWithDetailsAsync(long id)
        {
            return await Entities
                .Include(m => m.Manufacturer)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }
    }
}