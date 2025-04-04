using FCSP.DTOs;
using FCSP.DTOs.DesignService;
using FCSP.Models.Entities;

namespace FCSP.Services.DesignServiceService
{
    public interface IDesignServiceService
    {
        Task<BaseResponseModel<List<DesignServiceDto>>> GetAllDesignServices();
        Task<BaseResponseModel<GetDesignServiceByIdResponse>> GetDesignServiceById(GetDesignServiceByIdRequest request);
        Task<BaseResponseModel<AddDesignServiceResponse>> AddDesignService(AddDesignServiceRequest request);
        Task<BaseResponseModel<UpdateDesignServiceResponse>> UpdateDesignService(UpdateDesignServiceRequest request);
        Task<BaseResponseModel<DeleteDesignServiceResponse>> DeleteDesignService(DeleteDesignServiceRequest request);
        Task<BaseResponseModel<List<DesignServiceDto>>> GetDesignServicesByCustomShoeDesignId(GetDesignServicesByCustomShoeDesignIdRequest request);
        Task<BaseResponseModel<List<DesignServiceDto>>> GetDesignServicesByServiceId(GetDesignServicesByServiceIdRequest request);
    }
}