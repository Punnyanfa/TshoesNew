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
                .Where(m => m.UserId == userId && m.IsDeleted != false)
                .Include(m => m.Services)
                .ThenInclude(s => s.SetServiceAmounts)
                .Include(m => m.ManufacturerCriterias)
                .ThenInclude(mc => mc.Criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturersByStatusAsync(int status)
        {
            return await Entities
                .Where(m => m.Status == (ManufacturerStatus)status && m.IsDeleted != false)
                .Include(m => m.Services)
                .ThenInclude(s => s.SetServiceAmounts)
                .Include(m => m.ManufacturerCriterias)
                .ThenInclude(mc => mc.Criteria)
                .ToListAsync();
        }

        public async Task<Manufacturer> GetManufacturerWithDetailsAsync(long id)
        {
            return await Entities               
                .Include(m => m.Services)
                .ThenInclude(s => s.SetServiceAmounts)
                .Include(m => m.ManufacturerCriterias)
                .ThenInclude(mc => mc.Criteria)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted != false);
        }
        public async Task<List<Manufacturer>> GetAllWithDetailsAsync()
        {
            return await _context.Manufacturers
                .Where(m => m.IsDeleted != false)
                .Include(m => m.Services)
                .ThenInclude(s => s.SetServiceAmounts)
                .Include(m => m.ManufacturerCriterias)
                .ThenInclude(mc => mc.Criteria)
                .ToListAsync();
        }
    }
}