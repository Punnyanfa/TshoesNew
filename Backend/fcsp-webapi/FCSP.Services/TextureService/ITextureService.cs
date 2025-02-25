using FCSP.DTOs.Texture;
using FCSP.Models.Entities;

namespace FCSP.Services.TextureService
{
    public interface ITextureService
    {
        Task<IEnumerable<Texture>> GetAllTexture();
        Task<GetTextureByIdResponse> GetTextureById(GetTextureByIdRequest request);
        Task<GetTextureByIdResponse> AddTexture(AddTextureRequest request);
        Task<GetTextureByIdResponse> UpdateTexture(UpdateTextureRequest request);
        Task<GetTextureByIdResponse> DeleteTexture(DeleteTextureRequest request);
    }
}
