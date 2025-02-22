using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.Repositories;

namespace FCSP.Services.TextureService
{
    public class TextureService : ITextureService
    {
        private readonly ITextureRepository _textureRepository;

        public TextureService(ITextureRepository textureRepository)
        {
            _textureRepository = textureRepository;
        }

        public async Task<IEnumerable<Texture>> GetAllTemplate()
        {
            var response = await _textureRepository.GetAllAsync();
            return response;
        }

        public async Task<GetTextureByIdResponse> GetTemplateById(GetTextureByIdRequest request)
        {
            Texture texture = GetEntityFromGetByIdRequest(request);
            return new GetTextureByIdResponse
            {
                UserId = texture.UserId,
                Price = texture.Price,
                Prompt = texture.Prompt,
                ImageUrl = texture.ImageUrl,
            };
        }

        public async Task<GetTextureByIdResponse> AddTemplate(AddTextureRequest request)
        {
            Texture customShoeDesignTemplate = GetEntityFromAddRequest(request);
            await _textureRepository.AddAsync(customShoeDesignTemplate);
            return new GetTextureByIdResponse();
        }

        public async Task<GetTextureByIdResponse> UpdateTemplate(UpdateTextureRequest request)
        {
            Texture customShoeDesignTemplate = GetEntityFromUpdateRequest(request);
            await _textureRepository.UpdateAsync(customShoeDesignTemplate);
            return new GetTextureByIdResponse();
        }

        public async Task<GetTextureByIdResponse> DeleteTemplate(DeleteTextureRequest request)
        {
            Texture customShoeDesignTemplate = GetEntityFromDeleteRequest(request);
            await _textureRepository.DeleteAsync(customShoeDesignTemplate.Id);
            return new GetTextureByIdResponse();
        }

        private Texture GetEntityFromGetByIdRequest(GetTextureByIdRequest request)
        {
            Texture template = _textureRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
            }
            return template;
        }

        private Texture GetEntityFromAddRequest(AddTextureRequest request)
        {
            return new Texture
            {
                UserId = request.UserId,
                Price = request.Price,
                Prompt = request.Prompt,
                ImageUrl = request.ImageUrl,
            };
        }

        private Texture GetEntityFromUpdateRequest(UpdateTextureRequest request)
        {
            Texture texture = _textureRepository.Find(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException();
            }
            texture.Price = request.Price ?? texture.Price;
            texture.UpdatedAt = DateTime.Now;
            return texture;
        }

        private Texture GetEntityFromDeleteRequest(DeleteTextureRequest request)
        {
            Texture template = _textureRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
            }
            return template;
        }
    }
}
