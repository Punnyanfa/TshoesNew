using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;

namespace FCSP.Services.OrderDetailService
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetails();
        Task<GetOrderDetailByIdResponse> GetOrderDetailById(GetOrderDetailByIdRequest request);
        Task<AddOrderDetailResponse> AddOrderDetail(AddOrderDetailRequest request);
        Task<AddOrderDetailResponse> UpdateOrderDetail(UpdateOrderDetailRequest request);
        Task<AddOrderDetailResponse> DeleteOrderDetail(DeleteOrderDetailRequest request);
    }
} 