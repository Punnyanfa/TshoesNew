using FCSP.DTOs.Texture;
using FCSP.Models.Entities;

namespace FCSP.Services.TextureService
{
    public interface ITextureService
    {
        Task<IEnumerable<Texture>> GetAllTemplate();
        Task<GetTextureByIdResponse> GetTemplateById(GetTextureByIdRequest request);
        Task<GetTextureByIdResponse> AddTemplate(AddTextureRequest request);
        Task<GetTextureByIdResponse> UpdateTemplate(UpdateTextureRequest request);
        Task<GetTextureByIdResponse> DeleteTemplate(DeleteTextureRequest request);
    }
}
