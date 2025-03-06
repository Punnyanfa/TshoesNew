using FCSP.DTOs.DesignService;
using FCSP.Models.Entities;

namespace FCSP.Services.DesignServiceService
{
    public interface IDesignServiceService
    {
        Task<IEnumerable<DesignService>> GetAllDesignServices();
        Task<GetDesignServiceByIdResponse> GetDesignServiceById(GetDesignServiceByIdRequest request);
        Task<AddDesignServiceResponse> AddDesignService(AddDesignServiceRequest request);
        Task<UpdateDesignServiceResponse> UpdateDesignService(UpdateDesignServiceRequest request);
        Task<DeleteDesignServiceResponse> DeleteDesignService(DeleteDesignServiceRequest request);
        Task<IEnumerable<DesignService>> GetDesignServicesByCustomShoeDesignId(long customShoeDesignId);
        Task<IEnumerable<DesignService>> GetDesignServicesByServiceId(float serviceId);
    }
} 