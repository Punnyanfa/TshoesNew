using FCSP.DTOs.Service;
using FCSP.Models.Entities;

namespace FCSP.Services.ServiceService
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServices();
        Task<GetServiceByIdResponse> GetServiceById(GetServiceByIdRequest request);
        Task<AddServiceResponse> AddService(AddServiceRequest request);
        Task<UpdateServiceResponse> UpdateService(UpdateServiceRequest request);
        Task<DeleteServiceResponse> DeleteService(DeleteServiceRequest request);
    }
} 