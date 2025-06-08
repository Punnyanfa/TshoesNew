using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<OrderDetail> GetByOrderIdAsync(long orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Size)
                .Include(od => od.CustomShoeDesign)
                    .ThenInclude(cd => cd.DesignPreviews)
                .Include(od => od.CustomShoeDesign)
                    .ThenInclude(cd => cd.DesignServices)
                        .ThenInclude(ds => ds.Service)
                .OrderByDescending(od => od.CreatedAt)
                .FirstOrDefaultAsync(od => od.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetByManufacturerIdAsync(long manufacturerIdId)
        {
            return await _context.OrderDetails
                .Where(od => od.ManufacturerId == manufacturerIdId)
                .Include(od => od.Size)
                .Include(od => od.CustomShoeDesign)
                    .ThenInclude(cd => cd.DesignPreviews)
                .Include(od => od.CustomShoeDesign)
                    .ThenInclude(cd => cd.DesignServices)
                        .ThenInclude(ds => ds.Service)
                .OrderByDescending(od => od.CreatedAt)
                .ToListAsync();
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