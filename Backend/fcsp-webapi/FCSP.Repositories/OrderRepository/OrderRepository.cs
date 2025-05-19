using FCSP.DTOs.Order;
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
                .Include(o => o.User)
                .Include(o => o.User)
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Size)
                .Include(o => o.Payments)
                .ToListAsync();
        }
    
        public async Task<Order?> GetOrderByIdAsync(long orderId)
        {
            return await Entities
                .Include(o => o.User)
                .Include(o => o.User)
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Size)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllPublicOrderAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }
    }
}