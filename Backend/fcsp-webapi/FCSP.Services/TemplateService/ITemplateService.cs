using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.DTOs;
using FCSP.Models.Entities;

namespace FCSP.Services.TemplateService
{
    public interface ITemplateService
    {
        Task<BaseResponseModel<List<CustomShoeDesignTemplate>>> GetAllTemplate();
        Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request);
        Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request);
        Task<BaseResponseModel<UpdateTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request); // Fixed return type
        Task<BaseResponseModel<DeleteTemplateResponse>> DeleteTemplate(DeleteTemplateRequest request);
        Task<BaseResponseModel<IEnumerable<long>>> GetCustomShoeDesignIdsByTemplate(long templateId);
        Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAvailableTemplates();
        Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> SearchTemplates(SearchTemplatesRequest request);
        Task<BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>> GetPopularTemplates(GetPopularTemplatesRequest request);
        Task<BaseResponseModel<UpdateTemplateStatusResponse>> UpdateTemplateStatus(UpdateTemplateStatusRequest request);
        Task<BaseResponseModel<GetTemplateStatsResponse>> GetTemplateStats(GetTemplateStatsRequest request);
    }
}