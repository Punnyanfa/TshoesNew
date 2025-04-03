using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;

namespace FCSP.Services.TemplateService
{
    public interface ITemplateService
    {
        Task<BaseResponseModel<List<CustomShoeDesignTemplate>>> GetAllTemplate();
        Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request);
        Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request);
        Task<BaseResponseModel<AddTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request);
        Task<BaseResponseModel<DeleteTemplateResponse>> DeleteTemplate(DeleteTemplateRequest request);
    }
}
