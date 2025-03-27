using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(long userId)
        {
            return await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(o => o.OrderDetails)
                .Include(o => o.ShippingStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllPublicOrderAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.ShippingStatus)
                .ToListAsync();
        }
    }
}