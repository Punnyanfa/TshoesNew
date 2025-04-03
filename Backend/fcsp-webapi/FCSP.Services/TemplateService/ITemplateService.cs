using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;

namespace FCSP.Services.TemplateService
{
    public interface ITemplateService
    {
        Task<IEnumerable<CustomShoeDesignTemplate>> GetAllTemplate();
        Task<GetTemplateByIdResponse> GetTemplateById(GetTemplateByIdRequest request);
        Task<AddTemplateResponse> AddTemplate(AddTemplateRequest request);
        Task<AddTemplateResponse> UpdateTemplate(UpdateTemplateRequest request);
        Task<DeleteTemplateResponse> DeleteTemplate(DeleteTemplateRequest request);
        Task<IEnumerable<long>> GetCustomShoeDesignIdsByTemplate(long templateId);
        Task<IEnumerable<CustomShoeDesignTemplate>> GetAvailableTemplates();
        Task<IEnumerable<CustomShoeDesignTemplate>> SearchTemplates(SearchTemplatesRequest request);
        Task<IEnumerable<GetPopularTemplatesResponse>> GetPopularTemplates(GetPopularTemplatesRequest request);
        Task<RestoreTemplateResponse> RestoreTemplate(RestoreTemplateRequest request);
        Task<GetTemplateStatsResponse> GetTemplateStats(GetTemplateStatsRequest request);
    }
}

