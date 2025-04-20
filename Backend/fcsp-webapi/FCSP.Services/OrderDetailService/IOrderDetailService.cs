using FCSP.DTOs;
using FCSP.DTOs.OrderDetail;

namespace FCSP.Services.OrderDetailService
{
    public interface IOrderDetailService
    {
        Task<BaseResponseModel<IEnumerable<GetOrderDetailByIdResponse>>> GetAllOrderDetails();
        Task<BaseResponseModel<GetOrderDetailByIdResponse>> GetOrderDetailById(GetOrderDetailByIdRequest request);
        Task<BaseResponseModel<AddOrderDetailResponse>> AddOrderDetail(AddOrderDetailRequest request);
        Task<BaseResponseModel<UpdateOrderDetailResponse>> UpdateOrderDetail(UpdateOrderDetailRequest request);
        Task<BaseResponseModel<DeleteOrderDetailResponse>> DeleteOrderDetail(DeleteOrderDetailRequest request);
    }
}