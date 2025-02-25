using FCSP.DTOs.Texture;
using FCSP.Models.Entities;

namespace FCSP.Services.TextureService
{
    public interface ITextureService
    {
        Task<IEnumerable<GetTextureByIdResponse>> GetAllTextures();
        Task<IEnumerable<GetTextureByIdResponse>> GetAvailableTextures();
        Task<GetTextureByIdResponse> GetTextureById(GetTextureByIdRequest request);
        Task<IEnumerable<GetTextureByIdResponse>> GetTexturesByUser(GetTexturesByUserRequest request);
        Task<AddTextureResponse> AddTexture(AddTextureRequest request);
        Task<GetTextureByIdResponse> UpdateTexture(UpdateTextureRequest request);
        Task<GetTextureByIdResponse> DeleteTexture(DeleteTextureRequest request);
    }
}
