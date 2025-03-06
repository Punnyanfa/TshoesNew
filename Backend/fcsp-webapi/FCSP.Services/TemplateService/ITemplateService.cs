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
    }
}
