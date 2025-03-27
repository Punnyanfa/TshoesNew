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
                .Where(si => si.UserId == userId && !si.IsDeleted)
                .ToListAsync();
        }
        public async Task<ShippingInfo> GetByOrderIdAsync(long orderId)
        {
            return await Entities
                .Include(si => si.Orders) // Include Orders navigation property
                .FirstOrDefaultAsync(si => si.Orders.Any(o => o.Id == orderId && !si.IsDeleted));
        }
    }
}