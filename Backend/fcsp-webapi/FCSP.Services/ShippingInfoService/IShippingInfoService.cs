using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;

namespace FCSP.Services.ShippingInfoService
{
    public interface IShippingInfoService
    {
        Task<IEnumerable<ShippingInfo>> GetAllShippingInfo();
        Task<GetShippingInfoByIdResponse> GetShippingInfoById(GetShippingInfoByIdRequest request);
        Task<GetShippingInfoByIdResponse> AddShippingInfo(AddShippingInfoRequest request);
        Task<GetShippingInfoByIdResponse> UpdateShippingInfo(UpdateShippingInfoRequest request);
        Task<GetShippingInfoByIdResponse> DeleteShippingInfo(DeleteShippingInfoRequest request);
    }
}
