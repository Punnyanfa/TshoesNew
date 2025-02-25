using FCSP.DTOs.CustomShoeDesign;
using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.CustomShoeDesignService
{
    public class CustomShoeDesignService : ICustomShoeDesignService
    {
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly ICustomShoeDesignTextureRepository _customShoeDesignTextureRepository;

        public CustomShoeDesignService(
            ICustomShoeDesignRepository customShoeDesignRepository,
            ICustomShoeDesignTextureRepository customShoeDesignTextureRepository)
        {
            _customShoeDesignRepository = customShoeDesignRepository;
            _customShoeDesignTextureRepository = customShoeDesignTextureRepository;
        }

        public async Task<IEnumerable<CustomShoeDesign>> GetAllCustomShoeDesigns()
        {
            var response = await _customShoeDesignRepository.GetAllAsync();
            return response;
        }

        public async Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromGetByIdRequest(request);
            return new GetCustomShoeDesignByIdResponse
            {
                Id = customShoeDesign.Id,
                UserId = customShoeDesign.UserId,
                CustomShoeDesignTemplateId = customShoeDesign.CustomShoeDesignTemplateId,
                DesignData = customShoeDesign.DesignData,
                Preview3DFileUrl = customShoeDesign.Preview3DFileUrl,
                Price = customShoeDesign.Price
            };
        }

        public async Task<GetCustomShoeDesignByIdResponse> AddCustomShoeDesign(AddCustomShoeDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromAddRequest(request);
            var addedDesign = await _customShoeDesignRepository.AddAsync(customShoeDesign);
            
            // Add textures if included in the request
            if (request.TextureIds != null && request.TextureIds.Any())
            {
                foreach (var textureId in request.TextureIds)
                {
                    await AddTextureToDesign(addedDesign.Id, textureId);
                }
            }
            
            return new GetCustomShoeDesignByIdResponse
            {
                Id = addedDesign.Id,
                UserId = addedDesign.UserId,
                CustomShoeDesignTemplateId = addedDesign.CustomShoeDesignTemplateId,
                DesignData = addedDesign.DesignData,
                Preview3DFileUrl = addedDesign.Preview3DFileUrl,
                Price = addedDesign.Price
            };
        }

        public async Task<GetCustomShoeDesignByIdResponse> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromUpdateRequest(request);
            await _customShoeDesignRepository.UpdateAsync(customShoeDesign);
            
            // Update textures if included in the request
            if (request.TextureIds != null)
            {
                // Remove existing textures
                await RemoveAllTexturesFromDesign(customShoeDesign.Id);
                
                // Add new textures
                foreach (var textureId in request.TextureIds)
                {
                    await AddTextureToDesign(customShoeDesign.Id, textureId);
                }
            }
            
            return new GetCustomShoeDesignByIdResponse
            {
                Id = customShoeDesign.Id,
                UserId = customShoeDesign.UserId,
                CustomShoeDesignTemplateId = customShoeDesign.CustomShoeDesignTemplateId,
                DesignData = customShoeDesign.DesignData,
                Preview3DFileUrl = customShoeDesign.Preview3DFileUrl,
                Price = customShoeDesign.Price
            };
        }

        public async Task<GetCustomShoeDesignByIdResponse> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromDeleteRequest(request);
            
            // Remove all textures first
            await RemoveAllTexturesFromDesign(customShoeDesign.Id);
            
            // Then delete the design
            await _customShoeDesignRepository.DeleteAsync(customShoeDesign.Id);
            
            return new GetCustomShoeDesignByIdResponse
            {
                Id = customShoeDesign.Id,
                UserId = customShoeDesign.UserId,
                CustomShoeDesignTemplateId = customShoeDesign.CustomShoeDesignTemplateId,
                DesignData = customShoeDesign.DesignData,
                Preview3DFileUrl = customShoeDesign.Preview3DFileUrl,
                Price = customShoeDesign.Price
            };
        }
        
        public async Task<IEnumerable<CustomShoeDesignTexture>> GetDesignTextures(long designId)
        {
            var allTextures = await _customShoeDesignTextureRepository.GetAllAsync();
            return allTextures.Where(t => t.CustomShoeDesignId == designId);
        }
        
        public async Task<CustomShoeDesignTexture> AddTextureToDesign(long designId, long textureId)
        {
            var texture = new CustomShoeDesignTexture
            {
                CustomShoeDesignId = designId,
                TextureId = textureId
            };
            
            return await _customShoeDesignTextureRepository.AddAsync(texture);
        }
        
        public async Task RemoveTextureFromDesign(long designId, long textureId)
        {
            var allTextures = await _customShoeDesignTextureRepository.GetAllAsync();
            var textureToRemove = allTextures
                .FirstOrDefault(t => t.CustomShoeDesignId == designId && t.TextureId == textureId);
                
            if (textureToRemove != null)
            {
                await _customShoeDesignTextureRepository.DeleteAsync(textureToRemove.Id);
            }
        }
        
        public async Task RemoveAllTexturesFromDesign(long designId)
        {
            var designTextures = (await GetDesignTextures(designId)).ToList();
            
            foreach (var texture in designTextures)
            {
                await _customShoeDesignTextureRepository.DeleteAsync(texture.Id);
            }
        }

        private CustomShoeDesign GetEntityFromGetByIdRequest(GetCustomShoeDesignByIdRequest request)
        {
            CustomShoeDesign design = _customShoeDesignRepository.Find(request.Id);
            if (design == null)
            {
                throw new InvalidOperationException("CustomShoeDesign not found");
            }
            return design;
        }

        private CustomShoeDesign GetEntityFromAddRequest(AddCustomShoeDesignRequest request)
        {
            return new CustomShoeDesign
            {
                UserId = request.UserId,
                CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId,
                DesignData = request.DesignData,
                Preview3DFileUrl = request.Preview3DFileUrl,
                Price = request.Price
            };
        }

        private CustomShoeDesign GetEntityFromUpdateRequest(UpdateCustomShoeDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = _customShoeDesignRepository.Find(request.Id);
            if (customShoeDesign == null)
            {
                throw new InvalidOperationException("CustomShoeDesign not found");
            }
            
            customShoeDesign.CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId ?? customShoeDesign.CustomShoeDesignTemplateId;
            customShoeDesign.DesignData = request.DesignData ?? customShoeDesign.DesignData;
            customShoeDesign.Price = request.Price ?? customShoeDesign.Price;
            customShoeDesign.Preview3DFileUrl = request.Preview3DFileUrl ?? customShoeDesign.Preview3DFileUrl;
            customShoeDesign.UpdatedAt = DateTime.Now;
            
            return customShoeDesign;
        }

        private CustomShoeDesign GetEntityFromDeleteRequest(DeleteCustomShoeDesignRequest request)
        {
            CustomShoeDesign design = _customShoeDesignRepository.Find(request.Id);
            if (design == null)
            {
                throw new InvalidOperationException("CustomShoeDesign not found");
            }
            return design;
        }
    }
} 