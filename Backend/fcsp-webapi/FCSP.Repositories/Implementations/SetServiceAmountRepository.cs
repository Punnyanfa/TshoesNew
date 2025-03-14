using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCSP.Common.Enums;

namespace FCSP.Repositories.Implementations
{
    public class SetServiceAmountRepository : GenericRepository<SetServiceAmount>, ISetServiceAmountRepository
    {
        public SetServiceAmountRepository(FcspDbContext context) : base(context)
        {
        }
        
        public async Task<IList<SetServiceAmount>> GetActiveServiceAmountsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await Entities
                .Where(ssa => ssa.Status == ServiceAmountStatus.Active && ssa.StartDate <= currentDate && (ssa.EndDate == null || ssa.EndDate >= currentDate))
                .Include(ssa => ssa.Service)
                .ToListAsync();
        }
        
        public async Task<IList<SetServiceAmount>> GetByServiceIdAsync(long serviceId)
        {
            return await Entities
                .Where(ssa => ssa.ServiceId == serviceId)
                .OrderByDescending(ssa => ssa.StartDate)
                .ToListAsync();
        }
        
        public async Task<SetServiceAmount> GetCurrentServiceAmountAsync(long serviceId)
        {
            var currentDate = DateTime.UtcNow;
            return await Entities
                .Where(ssa => ssa.ServiceId == serviceId && 
                       ssa.Status == ServiceAmountStatus.Active && 
                       ssa.StartDate <= currentDate && 
                       (ssa.EndDate == null || ssa.EndDate >= currentDate))
                .OrderByDescending(ssa => ssa.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<SetServiceAmount?> GetActiveAmountByServiceIdAsync(long serviceId)
        {
            var today = DateTime.UtcNow.Date;
            
            return await Entities
                .Where(ssa => ssa.ServiceId == serviceId 
                    && ssa.StartDate <= today 
                    && (ssa.EndDate == null || ssa.EndDate >= today)
                    && ssa.Status == ServiceAmountStatus.Active)
                .OrderByDescending(ssa => ssa.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SetServiceAmount>> GetCurrentAmountsByServiceIdAsync(long serviceId)
        {
            var today = DateTime.UtcNow.Date;
            
            return await Entities
                .Where(ssa => ssa.ServiceId == serviceId 
                    && ssa.StartDate <= today 
                    && (ssa.EndDate == null || ssa.EndDate >= today))
                .OrderByDescending(ssa => ssa.StartDate)
                .ToListAsync();
        }
    }
} 