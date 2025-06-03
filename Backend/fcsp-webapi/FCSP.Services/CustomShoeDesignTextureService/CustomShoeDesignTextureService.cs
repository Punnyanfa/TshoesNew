using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;

namespace FCSP.Services.CustomShoeDesignTextureService
{
    public class CustomShoeDesignTextureService : ICustomShoeDesignTextureService
    {
        private readonly ICustomShoeDesignTexturesRepository _customShoeDesignTexturesRepository;

        public CustomShoeDesignTextureService(ICustomShoeDesignTexturesRepository customShoeDesignTexturesRepository)
        {
            _customShoeDesignTexturesRepository = customShoeDesignTexturesRepository;
        }

        public async Task<IEnumerable<CustomShoeDesignTextures>> GetAllCustomShoeDesignTextures()
        {
            var response = await _customShoeDesignTexturesRepository.GetAllAsync();
            return response;
        }

        public async Task<GetCustomShoeDesignTextureByIdResponse> GetCustomShoeDesignTextureById(GetCustomShoeDesignTextureByIdRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await GetEntityFromGetByIdRequest(request);
            return new GetCustomShoeDesignTextureByIdResponse
            {
                CustomShoeDesignId = customShoeDesignTexture.CustomShoeDesignId,
                TextureId = customShoeDesignTexture.TextureId
            };
        }

        public async Task<AddCustomShoeDesignTextureResponse> AddCustomShoeDesignTexture(AddCustomShoeDesignTextureRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = GetEntityFromAddRequest(request);
            var addedCustomShoeDesignTexture = await _customShoeDesignTexturesRepository.AddAsync(customShoeDesignTexture);
            return new AddCustomShoeDesignTextureResponse { Id = addedCustomShoeDesignTexture.Id };
        }

        public async Task<AddCustomShoeDesignTextureResponse> UpdateCustomShoeDesignTexture(UpdateCustomShoeDesignTextureRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await GetEntityFromUpdateRequest(request);
            await _customShoeDesignTexturesRepository.UpdateAsync(customShoeDesignTexture);
            return new AddCustomShoeDesignTextureResponse { Id = customShoeDesignTexture.Id };
        }

        public async Task<AddCustomShoeDesignTextureResponse> DeleteCustomShoeDesignTexture(DeleteCustomShoeDesignTextureRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await GetEntityFromDeleteRequest(request);
            await _customShoeDesignTexturesRepository.DeleteAsync(customShoeDesignTexture.Id);
            return new AddCustomShoeDesignTextureResponse { Id = customShoeDesignTexture.Id };
        }

        private async Task<CustomShoeDesignTextures> GetEntityFromGetByIdRequest(GetCustomShoeDesignTextureByIdRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await _customShoeDesignTexturesRepository.FindAsync(request.Id);
            if (customShoeDesignTexture == null)
            {
                throw new InvalidOperationException("CustomShoeDesignTexture not found");
            }
            return customShoeDesignTexture;
        }

        private CustomShoeDesignTextures GetEntityFromAddRequest(AddCustomShoeDesignTextureRequest request)
        {
            return new CustomShoeDesignTextures
            {
                CustomShoeDesignId = request.CustomShoeDesignId,
                TextureId = request.TextureId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<CustomShoeDesignTextures> GetEntityFromUpdateRequest(UpdateCustomShoeDesignTextureRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await _customShoeDesignTexturesRepository.FindAsync(request.Id);
            if (customShoeDesignTexture == null)
            {
                throw new InvalidOperationException("CustomShoeDesignTexture not found");
            }

            customShoeDesignTexture.TextureId = request.TextureId;
            customShoeDesignTexture.UpdatedAt = DateTime.UtcNow;

            return customShoeDesignTexture;
        }

        private async Task<CustomShoeDesignTextures> GetEntityFromDeleteRequest(DeleteCustomShoeDesignTextureRequest request)
        {
            CustomShoeDesignTextures customShoeDesignTexture = await _customShoeDesignTexturesRepository.FindAsync(request.Id);
            if (customShoeDesignTexture == null)
            {
                throw new InvalidOperationException("CustomShoeDesignTexture not found");
            }
            return customShoeDesignTexture;
        }
    }
}