using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class CriteriaRepository : GenericRepository<Criteria>, ICriteriaRepository
    {
        public CriteriaRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Criteria>> GetActiveCriteriaAsync()
        {
            return await Entities
                .Where(c => c.Status == Common.Enums.CriteriaStatus.Active) // Assuming 1 means active
                .Include(c => c.ManufacturerCriterias)
                .ToListAsync();
        }
    }
}