using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.OrderDetailService
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetails();
        Task<GetOrderDetailByIdResponse> GetOrderDetailById(GetOrderDetailByIdRequest request);
        Task<AddOrderDetailResponse> AddOrderDetail(AddOrderDetailRequest request);
        Task<UpdateOrderDetailResponse> UpdateOrderDetail(UpdateOrderDetailRequest request);
        Task<DeleteOrderDetailResponse> DeleteOrderDetail(DeleteOrderDetailRequest request);
    }
} 