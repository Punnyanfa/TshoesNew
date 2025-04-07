using FCSP.DTOs;
using FCSP.DTOs.Order;
using System.Threading.Tasks;

namespace FCSP.Services.OrderService
{
    public interface IOrderService
    {
        Task<BaseResponseModel<List<GetOrderByIdResponse>>> GetAllOrders();
        Task<BaseResponseModel<GetOrderByIdResponse>> GetOrderById(GetOrderByIdRequest request);
        Task<BaseResponseModel<List<GetOrderByUserIdResponse>>> GetOrdersByUserId(GetOrdersByUserIdRequest request);
        Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request);
        Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request);
        Task<BaseResponseModel<DeleteOrderResponse>> DeleteOrder(DeleteOrderRequest request);
        Task<BaseResponseModel<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request);
        Task<BaseResponseModel<UpdateOrderStatusResponse>> UpdateOrderStatus(UpdateOrderStatusRequest request);
    }
}