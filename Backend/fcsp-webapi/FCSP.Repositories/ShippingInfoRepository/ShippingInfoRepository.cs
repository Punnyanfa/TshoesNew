using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class ShippingInfoRepository : GenericRepository<ShippingInfo>, IShippingInfoRepository
    {
        public ShippingInfoRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ShippingInfo>> GetByUserIdAsync(long userId)
        {
            return await Entities
                .Include(si => si.User)
                .Where(si => si.UserId == userId && !si.IsDeleted)
                .OrderByDescending(si => si.CreatedAt)
                .ToListAsync();
        }
        public async Task<ShippingInfo> GetByOrderIdAsync(long orderId)
        {
            return await Entities
                .Include(si => si.Orders).
                Include(si => si.User)
                .OrderByDescending(si => si.CreatedAt)
                .FirstOrDefaultAsync(si => si.Orders.Any(o => o.Id == orderId && !si.IsDeleted));
        }

        public async Task<IEnumerable<ShippingInfo>> GetAllAsync()
        {
            return await _context.ShippingInfos
                .Include(si => si.User)
                .Where(si => !si.IsDeleted)
                .OrderByDescending(si => si.CreatedAt)
                .ToListAsync();
        }
    }
}