using FCSP.DTOs;
using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;

namespace FCSP.Services.ShippingInfoService
{
    public interface IShippingInfoService
    {
        Task<BaseResponseModel<GetAllShippingInfoResponse>> GetAllShippingInfo();
        Task<BaseResponseModel<GetShippingInfoByIdResponse>> GetShippingInfoById(GetShippingInfoByIdRequest request);
        Task<BaseResponseModel<AddShippingInfoResponse>> AddShippingInfo(AddShippingInfoRequest request);
        Task<BaseResponseModel<UpdateShippingInfoResponse>> UpdateShippingInfo(UpdateShippingInfoRequest request);
        Task<BaseResponseModel<DeleteShippingInfoResponse>> DeleteShippingInfo(DeleteShippingInfoRequest request);
        Task<BaseResponseModel<GetShippingInfosByUserResponse>> GetShippingInfosByUserId(GetShippingInfosByUserRequest request);
        Task<BaseResponseModel<SetDefaultShippingInfoResponse>> SetDefaultShippingInfo(SetDefaultShippingInfoRequest request);
    }
}