using FCSP.Common.Enums;
using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<Manufacturer?> GetManufacturerByUserIdAsync(long userId)
        {
            return await Entities
                .Where(m => m.UserId == userId && m.Status == ManufacturerStatus.Active)
                .Include(m => m.Services)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturersByStatusAsync(int status)
        {
            return await Entities
                .Where(m => m.Status == (ManufacturerStatus)status && m.IsDeleted != true)
                .Include(m => m.Services)
                .ToListAsync();
        }

        public async Task<Manufacturer> GetManufacturerWithDetailsAsync(long id)
        {
            return await Entities               
                .Include(m => m.Services)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.Status == ManufacturerStatus.Active);
        }
        public async Task<List<Manufacturer>> GetAllWithDetailsAsync()
        {
            return await _context.Manufacturers
                .Include(m => m.Services)
                .Where(m => m.Status == ManufacturerStatus.Active)
                .ToListAsync();
        }
    }
}