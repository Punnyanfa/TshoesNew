using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;

namespace FCSP.Services.TemplateService
{
    public interface ITemplateService
    {
        Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAllTemplate();
        Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request);
        Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request);
        Task<BaseResponseModel<AddTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request);
        Task<BaseResponseModel<DeleteTemplateResponse>> DeleteTemplate(DeleteTemplateRequest request);
        Task<BaseResponseModel<IEnumerable<long>>> GetCustomShoeDesignIdsByTemplate(long templateId);
        Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAvailableTemplates();
        Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> SearchTemplates(SearchTemplatesRequest request);
        Task<BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>> GetPopularTemplates(GetPopularTemplatesRequest request);
        Task<BaseResponseModel<RestoreTemplateResponse>> RestoreTemplate(RestoreTemplateRequest request);
        Task<BaseResponseModel<GetTemplateStatsResponse>> GetTemplateStats(GetTemplateStatsRequest request);
    }
}