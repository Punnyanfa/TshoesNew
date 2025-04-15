using FCSP.DTOs;
using FCSP.DTOs.Criteria;
using System.Threading.Tasks;

namespace FCSP.Services.ManufacturerCriteriaService
{
    public interface IManufacturerCriteriaService
    {
        Task<BaseResponseModel<AddCriteriaResponse>> AddCriteriaAsync(AddCriteriaRequest request);
        Task<BaseResponseModel<GetCriteriaResponse>> GetCriteriaAsync(GetCriteriaRequest request);
        Task<BaseResponseModel<UpdateCriteriaResponse>> UpdateCriteriaAsync(UpdateCriteriaRequest request);
        Task<BaseResponseModel<bool>> DeleteCriteriaAsync(long id);
        Task<BaseResponseModel<List<GetCriteriaResponse>>> GetAllActiveCriteriaAsync();
    }
}