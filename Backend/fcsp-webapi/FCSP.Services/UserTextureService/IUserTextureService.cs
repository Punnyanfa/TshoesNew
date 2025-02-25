using FCSP.DTOs.UserTexture;
using FCSP.Models.Entities;

namespace FCSP.Services.UserTextureService
{
    public interface IUserTextureService
    {
        Task<IEnumerable<UserTexture>> GetAllUserTextures();
        Task<GetUserTextureByIdResponse> GetUserTextureById(GetUserTextureByIdRequest request);
        Task<IEnumerable<GetUserTextureByIdResponse>> GetUserTexturesByOwner(GetUserTexturesByOwnerRequest request);
        Task<IEnumerable<GetUserTextureByIdResponse>> GetUserTexturesByBuyer(GetUserTexturesByBuyerRequest request);
        Task<AddUserTextureResponse> AddUserTexture(AddUserTextureRequest request);
        Task<AddUserTextureResponse> UpdateUserTexture(UpdateUserTextureRequest request);
        Task<AddUserTextureResponse> DeleteUserTexture(DeleteUserTextureRequest request);
    }
} 