using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.Repositories.Texture;

namespace FCSP.Services.TextureService
{
    public class TextureService : ITextureService
    {
        private readonly ITextureRepository _textureRepository;

        public TextureService(ITextureRepository textureRepository)
        {
            _textureRepository = textureRepository;
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetAllTextures()
        {
            var textures = await _textureRepository.GetAllAsync();
            return textures.Select(MapToDetailedResponse);
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetAvailableTextures()
        {
            var textures = await _textureRepository.GetAvailableTexturesAsync();
            return textures.Select(MapToDetailedResponse);
        }

        public async Task<GetTextureByIdResponse> GetTextureById(GetTextureByIdRequest request)
        {
            Texture texture = GetEntityFromGetByIdRequest(request);
            return MapToDetailedResponse(texture);
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetTexturesByUser(GetTexturesByUserRequest request)
        {
            var textures = await _textureRepository.GetTexturesByUserIdAsync(request.UserId);
            return textures.Select(MapToDetailedResponse);
        }

        public async Task<AddTextureResponse> AddTexture(AddTextureRequest request)
        {
            Texture texture = GetEntityFromAddRequest(request);
            var addedTexture = await _textureRepository.AddAsync(texture);
            
            return new AddTextureResponse
            {
                Id = addedTexture.Id,
                UserId = addedTexture.UserId,
                Price = addedTexture.Price,
                ImageUrl = addedTexture.ImageUrl,
                Prompt = addedTexture.Prompt,
                CreatedAt = addedTexture.CreatedAt
            };
        }

        public async Task<GetTextureByIdResponse> UpdateTexture(UpdateTextureRequest request)
        {
            Texture texture = GetEntityFromUpdateRequest(request);
            await _textureRepository.UpdateAsync(texture);
            return MapToDetailedResponse(texture);
        }

        public async Task<GetTextureByIdResponse> DeleteTexture(DeleteTextureRequest request)
        {
            Texture texture = GetEntityFromDeleteRequest(request);
            var response = MapToDetailedResponse(texture);
            await _textureRepository.DeleteAsync(texture.Id);
            return response;
        }

        private GetTextureByIdResponse MapToDetailedResponse(Texture texture)
        {
            return new GetTextureByIdResponse
            {
                Id = texture.Id,
                UserId = texture.UserId,
                Price = texture.Price,
                ImageUrl = texture.ImageUrl,
                Prompt = texture.Prompt,
                CreatorName = texture.User?.Name,
                CreatedAt = texture.CreatedAt,
                UpdatedAt = texture.UpdatedAt,
                UsageCount = texture.CustomShoeDesignTextures?.Count() ?? 0
            };
        }

        private Texture GetEntityFromGetByIdRequest(GetTextureByIdRequest request)
        {
            Texture texture = _textureRepository.Find(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException("Texture not found");
            }
            return texture;
        }

        private Texture GetEntityFromAddRequest(AddTextureRequest request)
        {
            return new Texture
            {
                UserId = request.UserId,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Prompt = request.Prompt
            };
        }

        private Texture GetEntityFromUpdateRequest(UpdateTextureRequest request)
        {
            Texture texture = _textureRepository.Find(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException("Texture not found");
            }
            
            texture.Price = request.Price ?? texture.Price;
            texture.ImageUrl = request.ImageUrl ?? texture.ImageUrl;
            texture.Prompt = request.Prompt ?? texture.Prompt;
            texture.UpdatedAt = DateTime.Now;
            
            return texture;
        }

        private Texture GetEntityFromDeleteRequest(DeleteTextureRequest request)
        {
            Texture texture = _textureRepository.Find(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException("Texture not found");
            }
            return texture;
        }
    }
}
