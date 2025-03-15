using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.DTOs;

namespace FCSP.Services.TextureService
{
    public interface ITextureService
    {
        Task<BaseResponseModel<IEnumerable<GetTextureByIdResponse>>> GetAllTextures();
        Task<BaseResponseModel<IEnumerable<GetTextureByIdResponse>>> GetAvailableTextures();
        Task<BaseResponseModel<GetTextureByIdResponse>> GetTextureById(GetTextureByIdRequest request);
        Task<BaseResponseModel<IEnumerable<GetTextureByIdResponse>>> GetTexturesByUser(GetTexturesByUserRequest request);
        Task<BaseResponseModel<AddTextureResponse>> AddTexture(AddTextureRequest request);
        //Task<UpdateTextureResponse> UpdateTexture(UpdateTextureRequest request);
        Task<BaseResponseModel<DeleteTextureResponse>> DeleteTexture(DeleteTextureRequest request);
    }
}
