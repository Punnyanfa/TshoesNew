using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(long userId);
        Task<Order?> GetOrderByIdAsync(long orderId); 
        Task<IEnumerable<Order>> GetAllPublicOrderAsync();
    }
}