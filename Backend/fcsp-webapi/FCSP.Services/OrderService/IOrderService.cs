using FCSP.DTOs.Order;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.OrderService
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request);
        Task<AddOrderResponse> AddOrder(AddOrderRequest request);
        Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest request);
        Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request);
    }
} 