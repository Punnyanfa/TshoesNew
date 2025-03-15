using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class DesignServiceRepository : GenericRepository<DesignService>, IDesignServiceRepository
    {
        public DesignServiceRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DesignService>> GetDesignServicesByCustomShoeDesignId(long customShoeDesignId)
        {
            return await Entities
                .Where(ds => ds.CustomShoeDesign != null && ds.CustomShoeDesign.Id == customShoeDesignId)
                .Include(ds => ds.CustomShoeDesign)
                .Include(ds => ds.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<DesignService>> GetDesignServicesByServiceId(long serviceId)
        {
            return await Entities
                .Where(ds => ds.ServiceId == serviceId)
                .Include(ds => ds.CustomShoeDesign)
                .Include(ds => ds.Service)
                .ToListAsync();
        }
    }
}