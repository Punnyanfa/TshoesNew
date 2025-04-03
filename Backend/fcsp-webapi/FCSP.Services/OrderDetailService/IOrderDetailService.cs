using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

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