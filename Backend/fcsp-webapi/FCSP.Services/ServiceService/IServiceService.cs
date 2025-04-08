using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;

namespace FCSP.Services.ServiceService
{
    public interface IServiceService
    {
        Task<BaseResponseModel<List<Service>>> GetAllServices();
        Task<BaseResponseModel<GetServiceByIdResponse>> GetServiceById(GetServiceByIdRequest request);
        Task<BaseResponseModel<AddServiceResponse>> AddService(AddServiceRequest request);
        Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request);
        Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request);
    }
}