using FCSP.DTOs;
using FCSP.DTOs.ManufacturerCriteria;
using System.Threading.Tasks;

namespace FCSP.Services.ManufacturerCriteriaService
{
    public interface IManufacturerCriteriaService
    {
        Task<BaseResponseModel<AddManufacturerCriteriaResponse>> AddManufacturerCriteriaAsync(AddManufacturerCriteriaRequest request);
        Task<BaseResponseModel<GetManufacturerCriteriaResponse>> GetManufacturerCriteriaAsync(GetManufacturerCriteriaRequest request);
        Task<BaseResponseModel<UpdateManufacturerCriteriaResponse>> UpdateManufacturerCriteriaAsync(UpdateManufacturerCriteriaRequest request);
        Task<BaseResponseModel<bool>> DeleteManufacturerCriteriaAsync(long id);
        Task<BaseResponseModel<List<GetManufacturerCriteriaResponse>>> GetAllManufacturerCriteriaAsync();
    }
}