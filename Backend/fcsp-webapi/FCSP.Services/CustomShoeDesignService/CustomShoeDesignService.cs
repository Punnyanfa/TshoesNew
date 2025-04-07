using FCSP.DTOs.CustomShoeDesign;
using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.CustomShoeDesignService;

public class CustomShoeDesignService : ICustomShoeDesignService
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
    private readonly ICustomShoeDesignTextureRepository _customShoeDesignTextureRepository;
    private readonly IDesignPreviewRepository _designPreviewRepository;
    private readonly ICustomShoeDesignTexturesRepository _customShoeDesignTexturesRepository;
    private readonly ICustomShoeDesignTemplateRepository _customShoeDesignTemplateRepository;
    private readonly IDesignServiceRepository _designServiceRepository;
    private readonly ISetServiceAmountRepository _setServiceAmountRepository;
    private readonly IServiceRepository _serviceRepository;

    public CustomShoeDesignService(
        ICustomShoeDesignRepository customShoeDesignRepository,
        IOrderDetailRepository orderDetailRepository,
        ICustomShoeDesignTextureRepository customShoeDesignTextureRepository,
        IDesignPreviewRepository designPreviewRepository,
        ICustomShoeDesignTexturesRepository customShoeDesignTexturesRepository,
        ICustomShoeDesignTemplateRepository customShoeDesignTemplateRepository,
        IDesignServiceRepository designServiceRepository,
        ISetServiceAmountRepository setServiceAmountRepository,
        IServiceRepository serviceRepository)
    {
        _customShoeDesignRepository = customShoeDesignRepository;
        _orderDetailRepository = orderDetailRepository;
        _customShoeDesignTextureRepository = customShoeDesignTextureRepository;
        _designPreviewRepository = designPreviewRepository;
        _customShoeDesignTexturesRepository = customShoeDesignTexturesRepository;
        _customShoeDesignTemplateRepository = customShoeDesignTemplateRepository;
        _designServiceRepository = designServiceRepository;
        _setServiceAmountRepository = setServiceAmountRepository;
        _serviceRepository = serviceRepository;
    }

    #region Public Methods
    public async Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetAllPublicDesigns()
    {
        try{
            var designs = await GetAllPublicCustomShoeDesigns();
            if(designs == null || !designs.Any())
            {
                return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
                {
                    Code = 404,
                    Message = "No public custom shoe designs found",
                    Data = null
                };
            }

            return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
            {
                Code = 200,
                Message = "Custom shoe designs retrieved successfully",
                Data = new GetAllPublicCustomShoeDesignsResponse
                {
                    Designs = designs
                }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetTopFiveBestSellingPublicDesigns()
    {
        try
        {
            var designs = await GetTopFiveBestSellingDesigns();
            if(designs == null || !designs.Any())
            {
                return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
                {
                    Code = 404,
                    Message = "No public custom shoe designs found",
                    Data = null
                };
            }

            return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
            {
                Code = 200,
                Message = "Custom shoe designs retrieved successfully",
                Data = new GetAllPublicCustomShoeDesignsResponse
                {
                    Designs = designs
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetCustomShoeDesignByIdResponse>> GetDesignById(GetCustomShoeDesignByIdRequest request)
    {
        try{
            var design = await GetCustomShoeDesignById(request);
            if(design == null)
            {
                return new BaseResponseModel<GetCustomShoeDesignByIdResponse>
                {
                    Code = 404,
                    Message = "Custom shoe design not found",
                    Data = null
                };
            }

            return new BaseResponseModel<GetCustomShoeDesignByIdResponse>
            {
                Code = 200,
                Message = "Custom shoe design retrieved successfully",
                Data = design
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<GetCustomShoeDesignByIdResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetListCustomShoeDesignsResponse>> GetDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request)
    {
        try{
            var designs = await GetCustomShoeDesignsByUserId(request);
            if(designs == null || !designs.Any())
            {
                return new BaseResponseModel<GetListCustomShoeDesignsResponse>
                {
                    Code = 404,
                    Message = "No custom shoe designs found for user with ID {request.UserId}",
                    Data = null
                };
            }

            return new BaseResponseModel<GetListCustomShoeDesignsResponse>
            {
                Code = 200,
                Message = "Custom shoe designs retrieved successfully",
                Data = new GetListCustomShoeDesignsResponse
                {
                    Designs = designs
                }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<GetListCustomShoeDesignsResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<AddCustomShoeDesignResponse>> AddCustomShoeDesign(AddCustomShoeDesignRequest request)
    {
        try{
            var design = await GetCustomShoeDesignFromAddDesignRequest(request);
        
            var addedDesign = await _customShoeDesignRepository.AddAsync(design);

            var designTextures = GetDesignTexturesFromAddDesignRequest(request, addedDesign.Id);

            var designServices = GetDesignServicesFromAddDesignRequest(request, addedDesign.Id);

            if(designTextures.Any())
            {
                await _customShoeDesignTextureRepository.AddRangeAsync(designTextures);
            }

            if(designServices.Any())
            {
                await _designServiceRepository.AddRangeAsync(designServices);
            }

            return new BaseResponseModel<AddCustomShoeDesignResponse> 
            { 
                Code = 200,
                Message = "Custom shoe design added successfully",
                Data = new AddCustomShoeDesignResponse 
                { 
                    Success = true
                }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<AddCustomShoeDesignResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new AddCustomShoeDesignResponse
                {
                    Success = false
                }
            };
        }
    }

    public async Task<BaseResponseModel<UpdateCustomShoeDesignResponse>> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request)
    { 
        try{
            var design = await GetCustomShoeDesignFromUpdateDesignRequest(request);

            await _customShoeDesignRepository.UpdateAsync(design);

            await UpdateCustomShoeDesignTextures(request);

            return new BaseResponseModel<UpdateCustomShoeDesignResponse> 
            { 
                Code = 200,
                Message = "Custom shoe design updated successfully",
                Data = new UpdateCustomShoeDesignResponse 
                { 
                    Success = true
                }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<UpdateCustomShoeDesignResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateCustomShoeDesignResponse
                {
                    Success = false
                }
            };
        }
    }

    public async Task<BaseResponseModel<DeleteCustomShoeDesignResponse>> DeleteCustomShoeDesign(DeleteCustomShoeDesignRequest request)
    {
        try{
            var design = await GetCustomShoeDesignFromDeleteDesignRequest(request);

        await _customShoeDesignRepository.UpdateAsync(design);

        return new BaseResponseModel<DeleteCustomShoeDesignResponse>
        {
            Code = 200,
            Message = "Custom shoe design deleted successfully",
                Data = new DeleteCustomShoeDesignResponse { Success = true }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<DeleteCustomShoeDesignResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new DeleteCustomShoeDesignResponse { Success = false }
            };
        }
    }
    #endregion

    #region Private Methods
    
    private async Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetAllPublicCustomShoeDesigns()
    {
        var designs = await _customShoeDesignRepository.GetAllPublicCustomShoeDesignsAsync();
        if(designs == null || !designs.Any())
        {
            return new List<GetCustomShoeDesignByIdResponse>();
        }
        return designs.Select(d => new GetCustomShoeDesignByIdResponse
        {
            Id = d.Id,
            Name = d.CustomShoeDesignTemplate?.Name,
            Description = d.Description,
            Gender = d.CustomShoeDesignTemplate?.Gender,
            Size = d.Size?.SizeValue,
            Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
            RatingCount = d.Ratings?.Count ?? 0,
            PreviewImageUrl = d.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
            Price = d.TotalAmount
        });
    }

    private async Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetTopFiveBestSellingDesigns()
    {
        var designIds = await _orderDetailRepository.GetTopFiveBestSellingDesignsAsync();
        var designs = await _customShoeDesignRepository.GetDesignsByIdsAsync(designIds);
        if(designs == null || !designs.Any())
        {
            return new List<GetCustomShoeDesignByIdResponse>();
        }
        return designs.Select(d => new GetCustomShoeDesignByIdResponse
        {
            Id = d.Id,
            Name = d.CustomShoeDesignTemplate?.Name,
            Description = d.Description,
            Gender = d.CustomShoeDesignTemplate?.Gender,
            Size = d.Size?.SizeValue,
            Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
            RatingCount = d.Ratings?.Count ?? 0,
            PreviewImageUrl = d.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
            Price = d.TotalAmount
        });
    }

    private async Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request)
    {
        var design = await _customShoeDesignRepository.FindAsync(request.Id);
        return new GetCustomShoeDesignByIdResponse
        {
            Id = design.Id,
            Name = design.CustomShoeDesignTemplate?.Name,
            Description = design.Description,
            Gender = design.CustomShoeDesignTemplate?.Gender,
            Size = design.Size?.SizeValue,
            Rating = design.Ratings != null && design.Ratings.Any() ? (float)Math.Round(design.Ratings.Average(r => r.UserRating), 1) : 0,
            RatingCount = design.Ratings?.Count ?? 0,
            PreviewImageUrl = design.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
            Price = design.TotalAmount
        };
    }
    
    private async Task<IEnumerable<GetCustomShoeDesignByIdResponse>> GetCustomShoeDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request)
    {
        var designs = await _customShoeDesignRepository.GetDesignsByUserIdAsync(request.UserId);
        if(designs == null || !designs.Any())
        {
            return new List<GetCustomShoeDesignByIdResponse>();
        }
        return designs.Select(d => new GetCustomShoeDesignByIdResponse
        {
            Id = d.Id,
            Name = d.CustomShoeDesignTemplate?.Name,
            PreviewImageUrl = d.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
            Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
            RatingCount = d.Ratings?.Count ?? 0,
            Price = d.TotalAmount
        });
    }

    private async Task<CustomShoeDesign> GetCustomShoeDesignFromAddDesignRequest(AddCustomShoeDesignRequest request)
    {
        
        var template = await _customShoeDesignTemplateRepository.FindAsync(request.CustomShoeDesignTemplateId ?? 0);
        var templatePrice = template?.Price ?? 0;
        
        float servicesPrice = 0;
        if (request.ServiceIds != null && request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var serviceAmount = await _setServiceAmountRepository.GetActiveAmountByServiceIdAsync(serviceId);
                if (serviceAmount != null)
                {
                    servicesPrice += serviceAmount.Amount;
                }
            }
        }
        
        var totalAmount = templatePrice + servicesPrice;
        var design = new CustomShoeDesign
        {
            UserId = request.UserId ?? 0,
            CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId ?? 0,
            DesignData = request.DesignData,
            Description = request.Description,
            Status = Common.Enums.CustomShoeDesignStatus.Private,
            DesignerMarkup = request.DesignerMarkup ?? 0,
            TotalAmount = totalAmount,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return design;
    }

    private IEnumerable<CustomShoeDesignTextures> GetDesignTexturesFromAddDesignRequest(AddCustomShoeDesignRequest request, long designId)
    {
        if(request.TextureIds == null || !request.TextureIds.Any())
        {
            return new List<CustomShoeDesignTextures>();
        }
        return request.TextureIds.Select(textureId => new CustomShoeDesignTextures
        {
            CustomShoeDesignId = designId,
            TextureId = textureId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    private IEnumerable<DesignService> GetDesignServicesFromAddDesignRequest(AddCustomShoeDesignRequest request, long designId)
    {
        if(request.ServiceIds == null || !request.ServiceIds.Any())
        {
            return new List<DesignService>();
        }
        return request.ServiceIds.Select(serviceId => new DesignService
        {
            CustomShoeDesignId = designId,
            ServiceId = serviceId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    private async Task<CustomShoeDesign> GetCustomShoeDesignFromUpdateDesignRequest(UpdateCustomShoeDesignRequest request)
    {
        var design = await _customShoeDesignRepository.FindAsync(request.Id);
        if (design == null)
        {
            throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
        }

        var template = await _customShoeDesignTemplateRepository.FindAsync(request.CustomShoeDesignTemplateId);
        if (template == null)
        {
            throw new InvalidOperationException($"Custom shoe design template with ID {request.CustomShoeDesignTemplateId} not found");
        }
        
        design.DesignData = request.DesignData ?? design.DesignData;
        design.UpdatedAt = DateTime.UtcNow;
        return design;
    }
    
    private async Task UpdateCustomShoeDesignTextures(UpdateCustomShoeDesignRequest request)
    {
        var existingTextures = await _customShoeDesignTexturesRepository.GetByCustomShoeDesignIdAsync(request.Id);
        var existingTextureIds = existingTextures.Select(t => t.TextureId).ToList();

        if (request.TextureIds == null || !request.TextureIds.Any())
        {
            await _customShoeDesignTextureRepository.RemoveRangeAsync(existingTextures.Select(t => t.Id));
            return;
        }

        var removeTextureIds = existingTextureIds.Except(request.TextureIds).ToList();
        
        if (removeTextureIds.Any())
        {
            await _customShoeDesignTextureRepository.RemoveRangeAsync(removeTextureIds);
        }

        var addTextureIds = request.TextureIds.Except(existingTextureIds).ToList();
        
        if (addTextureIds.Any())
        {
            var newTextures = addTextureIds.Select(textureId => new CustomShoeDesignTextures
            {
                CustomShoeDesignId = request.Id,
                TextureId = textureId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _customShoeDesignTextureRepository.AddRangeAsync(newTextures);
        }
    }
    
    private async Task<CustomShoeDesign> GetCustomShoeDesignFromDeleteDesignRequest(DeleteCustomShoeDesignRequest request)
    {
        var design = await _customShoeDesignRepository.FindAsync(request.Id);

        if (design == null)
        {
            throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
        }

        design.IsDeleted = true;
        design.UpdatedAt = DateTime.UtcNow;
        design.Status = Common.Enums.CustomShoeDesignStatus.Archived;
        
        return design;
    }
    #endregion
}
