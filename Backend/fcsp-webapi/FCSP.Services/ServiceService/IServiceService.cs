using FCSP.DTOs;
using FCSP.DTOs.Service;
using System.Threading.Tasks;

namespace FCSP.Services.ServiceService
{
    public interface IServiceService
    {
        Task<BaseResponseModel<List<ServiceResponseDto>>> GetAllServices();
        Task<BaseResponseModel<ServiceResponseDto>> GetServiceById(GetServiceByIdRequest request);
        Task<BaseResponseModel<AddServiceResponse>> AddService(AddServiceRequest request);
        Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request);
        Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request);
    }
}