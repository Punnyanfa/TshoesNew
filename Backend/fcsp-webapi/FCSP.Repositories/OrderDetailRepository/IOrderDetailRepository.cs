using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<OrderDetail> GetByOrderIdAsync(long orderId);
        Task<IEnumerable<OrderDetail>> GetByManufacturerIdAsync(long manufacturerIdId);
        Task<IEnumerable<long>> GetTopFiveBestSellingDesignsAsync();
    }
}