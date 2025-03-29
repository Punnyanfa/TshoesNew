using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface IShippingInfoRepository : IGenericRepository<ShippingInfo>
    {
        Task<IEnumerable<ShippingInfo>> GetAllAsync();
        Task<IEnumerable<ShippingInfo>> GetByUserIdAsync(long userId);
        Task<ShippingInfo> GetByOrderIdAsync(long orderId);
    }
}