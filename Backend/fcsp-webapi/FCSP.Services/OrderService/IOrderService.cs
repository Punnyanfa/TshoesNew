using FCSP.DTOs.Order;
using FCSP.Models.Entities;

namespace FCSP.Services.OrderService
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request);
        Task<AddOrderResponse> AddOrder(AddOrderRequest request);
        Task<AddOrderResponse> UpdateOrder(UpdateOrderRequest request);
        Task<AddOrderResponse> DeleteOrder(DeleteOrderRequest request);
    }
} 