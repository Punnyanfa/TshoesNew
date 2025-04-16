using FCSP.DTOs;
using FCSP.DTOs.Criteria;
using System.Threading.Tasks;

namespace FCSP.Services.CriteriaService
{
    public interface ICriteriaService
    {
        Task<BaseResponseModel<AddCriteriaResponse>> AddAsync(AddCriteriaRequest request);
        Task<BaseResponseModel<UpdateCriteriaResponse>> UpdateAsync(UpdateCriteriaRequest request);
        Task<BaseResponseModel<UpdateCriteriaStatusResponse>> UpdateStatusAsync(UpdateCriteriaStatusRequest request);
        Task<BaseResponseModel<bool>> DeleteAsync(long id);
        Task<BaseResponseModel<IList<GetCriteriaResponse>>> GetAllAsync();
        Task<BaseResponseModel<GetCriteriaResponse>> GetByIdAsync(long id);
    }
}