using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class ManufacturerCriteriaRepository : GenericRepository<ManufacturerCriteria>, IManufacturerCriteriaRepository
    {
        public ManufacturerCriteriaRepository(FcspDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<ManufacturerCriteria>> GetByManufacturerIdAsync(long manufacturerId)
        {
            return await Entities
                .Where(mc => mc.ManufacturerId == manufacturerId)
                .Include(mc => mc.Criteria)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<ManufacturerCriteria>> GetByCriteriaIdAsync(long criteriaId)
        {
            return await Entities
                .Where(mc => mc.CriteriaId == criteriaId)
                .Include(mc => mc.Manufacturer)
                .ToListAsync();
        }
    }
} 