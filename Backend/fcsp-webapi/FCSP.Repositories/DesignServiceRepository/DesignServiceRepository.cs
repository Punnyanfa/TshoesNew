using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class DesignServiceRepository : GenericRepository<DesignService>, IDesignServiceRepository
    {
        private readonly FcspDbContext _dbContext;

        public DesignServiceRepository(FcspDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<DesignService>> GetServicesByCustomShoeDesignIdAsync(long customShoeDesignId)
        {
            return await _dbContext.DesignServices.Where(ds => ds.CustomShoeDesignId == customShoeDesignId).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<DesignService> entities)
        {
            await _dbContext.DesignServices.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<long> serviceIds)
        {
            var designServicesToDelete = await _dbContext.DesignServices.Where(ds => serviceIds.Contains(ds.ServiceId)).ToListAsync();
            _dbContext.DesignServices.RemoveRange(designServicesToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}