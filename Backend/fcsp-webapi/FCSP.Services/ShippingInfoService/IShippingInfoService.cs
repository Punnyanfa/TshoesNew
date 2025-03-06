using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.ShippingInfoService
{
    public interface IShippingInfoService
    {
        Task<IEnumerable<ShippingInfo>> GetAllShippingInfo();
        Task<GetShippingInfoByIdResponse> GetShippingInfoById(GetShippingInfoByIdRequest request);
        Task<AddShippingInfoResponse> AddShippingInfo(AddShippingInfoRequest request);
        Task<UpdateShippingInfoResponse> UpdateShippingInfo(UpdateShippingInfoRequest request);
        Task<DeleteShippingInfoResponse> DeleteShippingInfo(DeleteShippingInfoRequest request);
    }
}
