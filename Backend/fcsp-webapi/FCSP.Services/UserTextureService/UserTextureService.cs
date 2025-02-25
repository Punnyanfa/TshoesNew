using FCSP.DTOs.UserTexture;
using FCSP.Models.Entities;
using FCSP.Repositories.UserTexture;

namespace FCSP.Services.UserTextureService
{
    public class UserTextureService : IUserTextureService
    {
        private readonly IUserTextureRepository _userTextureRepository;

        public UserTextureService(IUserTextureRepository userTextureRepository)
        {
            _userTextureRepository = userTextureRepository;
        }

        public async Task<IEnumerable<UserTexture>> GetAllUserTextures()
        {
            return await _userTextureRepository.GetAllAsync();
        }

        public async Task<GetUserTextureByIdResponse> GetUserTextureById(GetUserTextureByIdRequest request)
        {
            UserTexture userTexture = GetEntityFromGetByIdRequest(request);
            return MapToResponse(userTexture);
        }

        public async Task<IEnumerable<GetUserTextureByIdResponse>> GetUserTexturesByOwner(GetUserTexturesByOwnerRequest request)
        {
            var userTextures = await _userTextureRepository.GetTexturesByOwnerIdAsync(request.OwnerId);
            return userTextures.Select(MapToResponse);
        }

        public async Task<IEnumerable<GetUserTextureByIdResponse>> GetUserTexturesByBuyer(GetUserTexturesByBuyerRequest request)
        {
            var userTextures = await _userTextureRepository.GetTexturesByBuyerIdAsync(request.BuyerId);
            return userTextures.Select(MapToResponse);
        }

        public async Task<AddUserTextureResponse> AddUserTexture(AddUserTextureRequest request)
        {
            UserTexture userTexture = GetEntityFromAddRequest(request);
            var addedUserTexture = await _userTextureRepository.AddAsync(userTexture);
            return new AddUserTextureResponse { Id = addedUserTexture.Id };
        }

        public async Task<AddUserTextureResponse> UpdateUserTexture(UpdateUserTextureRequest request)
        {
            UserTexture userTexture = GetEntityFromUpdateRequest(request);
            await _userTextureRepository.UpdateAsync(userTexture);
            return new AddUserTextureResponse { Id = userTexture.Id };
        }

        public async Task<AddUserTextureResponse> DeleteUserTexture(DeleteUserTextureRequest request)
        {
            UserTexture userTexture = GetEntityFromDeleteRequest(request);
            await _userTextureRepository.DeleteAsync(userTexture.Id);
            return new AddUserTextureResponse { Id = userTexture.Id };
        }

        private GetUserTextureByIdResponse MapToResponse(UserTexture userTexture)
        {
            return new GetUserTextureByIdResponse
            {
                Id = userTexture.Id,
                OwnerId = userTexture.OwnerId,
                BuyerId = userTexture.BuyerId,
                TextureId = userTexture.TextureId,
                Status = userTexture.Status,
                TextureImageUrl = userTexture.Texture?.ImageUrl,
                TexturePrice = userTexture.Texture?.Price,
                OwnerName = userTexture.Owner?.Name,
                BuyerName = userTexture.Buyer?.Name
            };
        }

        private UserTexture GetEntityFromGetByIdRequest(GetUserTextureByIdRequest request)
        {
            UserTexture userTexture = _userTextureRepository.Find(request.Id);
            if (userTexture == null)
            {
                throw new InvalidOperationException("UserTexture not found");
            }
            return userTexture;
        }

        private UserTexture GetEntityFromAddRequest(AddUserTextureRequest request)
        {
            return new UserTexture
            {
                OwnerId = request.OwnerId,
                BuyerId = request.BuyerId,
                TextureId = request.TextureId,
                Status = request.Status
            };
        }

        private UserTexture GetEntityFromUpdateRequest(UpdateUserTextureRequest request)
        {
            UserTexture userTexture = _userTextureRepository.Find(request.Id);
            if (userTexture == null)
            {
                throw new InvalidOperationException("UserTexture not found");
            }
            
            userTexture.Status = request.Status ?? userTexture.Status;
            userTexture.UpdatedAt = DateTime.Now;
            
            return userTexture;
        }

        private UserTexture GetEntityFromDeleteRequest(DeleteUserTextureRequest request)
        {
            UserTexture userTexture = _userTextureRepository.Find(request.Id);
            if (userTexture == null)
            {
                throw new InvalidOperationException("UserTexture not found");
            }
            return userTexture;
        }
    }
} 