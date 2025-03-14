using FCSP.DTOs.CustomShoeDesign;
using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetAllCustomShoeDesigns()
        {
            var designs = await _customShoeDesignRepository.GetAllAsync();
            return designs.Select(d => MapToDetailedResponse(d));
        }

        public async Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request)
        {
            var design = await _customShoeDesignRepository.FindAsync(request.Id);
            if (design == null)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
            }

            return MapToDetailedResponse(design);
        }

        public async Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetDesignsByUser(GetDesignsByUserRequest request)
        {
            var designs = await _customShoeDesignRepository.GetDesignsByUserIdAsync(request.UserId);
            return designs.Select(d => MapToDetailedResponse(d));
        }

        public async Task<AddCustomShoeDesignResponse> AddCustomShoeDesign(AddCustomShoeDesignRequest request)
        {
            var design = new CustomShoeDesign
            {
                UserId = request.UserId ?? 0,
                CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId ?? 0,
                DesignData = request.DesignData,
                Size = null, // Set appropriate default or get from request
                Status = 0, // Set appropriate default status
                DesignerMarkup = 0, // Set appropriate default or get from request
                TotalAmount = request.Price ?? 0,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedDesign = await _customShoeDesignRepository.AddAsync(design);

            if (request.TextureIds != null && request.TextureIds.Any())
            {
                var designTextures = request.TextureIds.Select(textureId => new CustomShoeDesignTexture
                {
                    CustomShoeDesignId = addedDesign.Id,
                    TextureId = textureId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                await _customShoeDesignTextureRepository.AddRangeAsync(designTextures);
            }

            return new AddCustomShoeDesignResponse 
            { 
                Id = addedDesign.Id,
                Preview3DFileUrl = null, // Set appropriate value if available
                Price = addedDesign.TotalAmount
            };
        }

        public async Task<UpdateCustomShoeDesignResponse> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request)
        { 
            var design = await _customShoeDesignRepository.FindAsync(request.Id);
            if (design == null)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
            }

            if (request.CustomShoeDesignTemplateId.HasValue)
            {
                design.CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId.Value;
            }
            
            design.DesignData = request.DesignData ?? design.DesignData;
            design.TotalAmount = request.Price ?? design.TotalAmount;
            design.UpdatedAt = DateTime.UtcNow;

            await _customShoeDesignRepository.UpdateAsync(design);

            if (request.TextureIds != null)
            {
                await _customShoeDesignTextureRepository.DeleteByDesignIdAsync(design.Id);

                if (request.TextureIds.Any())
                {
                    var designTextures = request.TextureIds.Select(textureId => new CustomShoeDesignTexture
                    {
                        CustomShoeDesignId = design.Id,
                        TextureId = textureId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

                    await _customShoeDesignTextureRepository.AddRangeAsync(designTextures);
                }
            }

            return new UpdateCustomShoeDesignResponse 
            { 
                Id = design.Id,
                Preview3DFileUrl = null, // Set appropriate value if available
                Price = design.TotalAmount
            };
        }

        public async Task<DeleteCustomShoeDesignResponse> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request)
        {
            var design = await _customShoeDesignRepository.FindAsync(request.Id);
            if (design == null)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
            }

            await _customShoeDesignTextureRepository.DeleteByDesignIdAsync(request.Id);
            await _customShoeDesignRepository.DeleteAsync(request.Id);

            return new DeleteCustomShoeDesignResponse { Success = true };
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
                TextureId = textureId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
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
            await _customShoeDesignTextureRepository.DeleteByDesignIdAsync(designId);
        }

        private GetCustomShoeDesignByIdResponse MapToDetailedResponse(CustomShoeDesign design)
        {
            return new GetCustomShoeDesignByIdResponse
            {
                Id = design.Id,
                UserId = design.UserId,
                CustomShoeDesignTemplateId = design.CustomShoeDesignTemplateId,
                DesignData = design.DesignData,
                Preview3DFileUrl = null, // Set appropriate value if available
                Price = design.TotalAmount,
                TextureIds = design.CustomShoeDesignTextures?.Select(t => t.TextureId).ToList() ?? new List<long>(),
                CreatedAt = design.CreatedAt,
                UpdatedAt = design.UpdatedAt
            };
        }
    }
} 