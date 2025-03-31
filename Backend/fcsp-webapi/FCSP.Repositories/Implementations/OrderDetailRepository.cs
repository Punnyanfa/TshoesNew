using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<long>> GetTopFiveBestSellingDesignsAsync()
        {
            return await _context.OrderDetails
                .Where(od => od.Order.Status == Common.Enums.OrderStatus.Completed)
                .Where(od => od.CustomShoeDesign.IsDeleted == false)
                .GroupBy(od => od.CustomShoeDesignId)
                .Select(g => new { DesignId = g.Key, TotalQuantity = g.Sum(od => od.Quantity) })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .Select(x => x.DesignId)
                .ToListAsync();
        }
    }
}