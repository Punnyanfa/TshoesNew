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
        Task<GenerateImageResponse> GenerateAndSaveImage(GenerateImageRequest request);
        Task<AddTextureResponse> AddTexture(AddTextureRequest request);
        Task<UpdateTextureResponse> UpdateTexture(UpdateTextureRequest request);
        Task<DeleteTextureResponse> DeleteTexture(DeleteTextureRequest request);
    }
}
