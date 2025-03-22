using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.DTOs;

namespace FCSP.Services.TextureService
{
    public interface ITextureService
    {
        Task<BaseResponseModel<GetAllTexturesResponse>> GetAllTextures();
        Task<BaseResponseModel<GetAvailableTexturesResponse>> GetAvailableTextures();
        Task<BaseResponseModel<GetTextureByIdResponse>> GetTextureById(GetTextureByIdRequest request);
        Task<BaseResponseModel<GetTexturesByUserIdResponse>> GetTexturesByUserId(GetTexturesByUserIdRequest request);
        Task<BaseResponseModel<AddTextureResponse>> AddTexture(AddTextureRequest request);
        Task<BaseResponseModel<DeleteTextureResponse>> DeleteTexture(DeleteTextureRequest request);
    }
}
