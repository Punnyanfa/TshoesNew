using FCSP.DTOs;
using FCSP.DTOs.Order;

namespace FCSP.Services.OrderService
{
    public interface IOrderService
    {
        Task<BaseResponseModel<GetOrdersByUserIdResponse>> GetOrdersByUserId(GetOrdersByUserIdRequest request);
        Task<BaseResponseModel<GetOrderByManufacturerIdResponse>> GetOrdersByManufacturerId(GetOrdersByManufacturerIdRequest request);
        Task<BaseResponseModel<GetOrderByIdResponse>> GetOrderById(GetOrderByIdRequest request);
        Task<BaseResponseModel<IEnumerable<GetOrderByIdResponse>>> GetAllOrders();
        Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request);
        Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrderStatus(UpdateOrderStatusRequest request);
        Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrderShippingStatus(UpdateOrderShippingStatusRequest request);
    }
}