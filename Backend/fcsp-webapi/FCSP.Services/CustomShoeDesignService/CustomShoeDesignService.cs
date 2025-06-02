using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesign;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;


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
    private readonly IServiceRepository _serviceRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IConfiguration _configuration;
    private readonly string? _azureConnectionString;
    private readonly string? _azureImageContainerName;
    private readonly string? _azureJsonContainerName;

    public CustomShoeDesignService(
        ICustomShoeDesignRepository customShoeDesignRepository,
        IOrderDetailRepository orderDetailRepository,
        ICustomShoeDesignTextureRepository customShoeDesignTextureRepository,
        IDesignPreviewRepository designPreviewRepository,
        ICustomShoeDesignTexturesRepository customShoeDesignTexturesRepository,
        ICustomShoeDesignTemplateRepository customShoeDesignTemplateRepository,
        IDesignServiceRepository designServiceRepository,
        IServiceRepository serviceRepository,
        ISizeRepository sizeRepository,
        IConfiguration configuration)
    {
        _customShoeDesignRepository = customShoeDesignRepository;
        _orderDetailRepository = orderDetailRepository;
        _customShoeDesignTextureRepository = customShoeDesignTextureRepository;
        _designPreviewRepository = designPreviewRepository;
        _customShoeDesignTexturesRepository = customShoeDesignTexturesRepository;
        _customShoeDesignTemplateRepository = customShoeDesignTemplateRepository;
        _designServiceRepository = designServiceRepository;
        _serviceRepository = serviceRepository;
        _sizeRepository = sizeRepository;
        _configuration = configuration;
        _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
        _azureImageContainerName = _configuration["AzureStorage:ImagesContainer"];
        _azureJsonContainerName = _configuration["AzureStorage:JsonContainer"];
    }

    #region Public Methods
    public async Task<BaseResponseModel<GetAllCustomShoeDesignResponse>> GetAllDesigns()
    {
        try
        {
            var designs = await GetAllCustomShoeDesigns();
            if(designs == null || !designs.Any())
            {
                return new BaseResponseModel<GetAllCustomShoeDesignResponse>
                {
                    Code = 404,
                    Message = "No custom shoe designs found",
                    Data = null
                };
            }
            return new BaseResponseModel<GetAllCustomShoeDesignResponse>
            {
                Code = 200,
                Message = "Custom shoe designs retrieved successfully",
                Data = new GetAllCustomShoeDesignResponse
                {
                    Designs = designs
                }
            };
        }   
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllCustomShoeDesignResponse>
            {
                Code = 500,
                Message = "Error retrieving custom shoe designs. Error: " + ex.Message,
                Data = null
            };
        } 
    }

    public async Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetAllPublicDesigns()
    {
        try
        {
            var designs = await GetAllPublicCustomShoeDesigns();
            if (designs == null || !designs.Any())
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

    public async Task<BaseResponseModel<GetAllPublicCustomShoeDesignsResponse>> GetTopFiveBestSellingPublicDesigns()
    {
        try
        {
            var designs = await GetTopFiveBestSellingDesigns();
            if (designs == null || !designs.Any())
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
        try
        {
            var design = await GetCustomShoeDesignById(request);
            if (design == null)
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
        catch (Exception ex)
        {
            return new BaseResponseModel<GetCustomShoeDesignByIdResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetCustomDesignsByUserIdResponse>> GetDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request)
    {
        try
        {
            var designs = await GetCustomShoeDesignsByUserId(request);
            if (designs == null || designs.Designs == null || !designs.Designs.Any())
            {
                return new BaseResponseModel<GetCustomDesignsByUserIdResponse>
                {
                    Code = 404,
                    Message = $"No custom shoe designs found for user with ID {request.UserId}",
                    Data = null
                };
            }

            return new BaseResponseModel<GetCustomDesignsByUserIdResponse>
            {
                Code = 200,
                Message = "Custom shoe designs retrieved successfully",
                Data = new GetCustomDesignsByUserIdResponse
                {
                    Designs = designs.Designs
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetCustomDesignsByUserIdResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<AddCustomShoeDesignResponse>> AddCustomShoeDesign(AddCustomShoeDesignRequest request)
    {
        try
        {
            var design = await GetCustomShoeDesignFromAddDesignRequest(request);

            var addedDesign = await _customShoeDesignRepository.AddAsync(design);

            var designPreviewImages = await GetDesignPreviewImagesFromAddDesignRequest(request, addedDesign.Id);

            var designTextures = GetDesignTexturesFromAddDesignRequest(request, addedDesign.Id);

            var designServices = GetDesignServicesFromAddDesignRequest(request, addedDesign.Id);

            if (designPreviewImages.Any())
            {
                await _designPreviewRepository.AddRangeAsync(designPreviewImages);
            }

            if (designTextures.Any())
            {
                await _customShoeDesignTextureRepository.AddRangeAsync(designTextures);
            }

            if (designServices.Any())
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
        catch (Exception ex)
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

    public async Task<BaseResponseModel<UpdateCustomShoeDesignStatusResponse>> UpdateCustomShoeDesignStatus(UpdateCustomShoeDesignStatusRequest request)
    {
        try
        {
            var design = await GetCustomShoeDesignFromUpdateDesignStatusRequest(request);

            await _customShoeDesignRepository.UpdateAsync(design);

            return new BaseResponseModel<UpdateCustomShoeDesignStatusResponse>
            {
                Code = 200,
                Message = "Custom shoe design status updated successfully",
                Data = new UpdateCustomShoeDesignStatusResponse
                {
                    Success = true
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateCustomShoeDesignStatusResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateCustomShoeDesignStatusResponse
                {
                    Success = false
                }
            };
        }
    }

    public async Task<BaseResponseModel<UpdateCustomShoeDesignResponse>> UpdateCustomShoeDesign(UpdateCustomShoeDesignRequest request)
    {
        try
        {
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
        catch (Exception ex)
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
        try
        {
            var design = await GetCustomShoeDesignFromDeleteDesignRequest(request);

            await _customShoeDesignRepository.UpdateAsync(design);

            return new BaseResponseModel<DeleteCustomShoeDesignResponse>
            {
                Code = 200,
                Message = "Custom shoe design deleted successfully",
                Data = new DeleteCustomShoeDesignResponse { Success = true }
            };
        }
        catch (Exception ex)
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
    private async Task<IEnumerable<GetSimpleCustomShoeDesignResponse>> GetAllCustomShoeDesigns()
    {
        var designs = await _customShoeDesignRepository.GetAll()
                                                        .Include(d => d.Ratings)
                                                        .Include(d => d.User)
                                                        .Include(d => d.CustomShoeDesignTemplate)
                                                        .Include(d => d.CustomShoeDesignTextures)
                                                            .ThenInclude(t => t.Texture)
                                                        .Include(d => d.DesignPreviews)
                                                        .Include(d => d.DesignServices)
                                                            .ThenInclude(s => s.Service)
                                                        .Where(d => d.Status != Common.Enums.CustomShoeDesignStatus.Private)
                                                        .Where(d => d.IsDeleted == false)
                                                        .ToListAsync();
        if (designs == null || !designs.Any())
        {
            return new List<GetSimpleCustomShoeDesignResponse>();
        }

        var responses = new List<GetSimpleCustomShoeDesignResponse>();
        foreach (var d in designs)
        {
            var totalAmount = await CalculateTotalAmount(d);
            responses.Add(new GetSimpleCustomShoeDesignResponse
            {
                Id = d.Id,
                Name = d.CustomShoeDesignTemplate?.Name,
                Gender = d.CustomShoeDesignTemplate?.Gender,
                Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
                Status = d.Status,
                RatingCount = d.Ratings?.Count ?? 0,
                PreviewImageUrl = d.DesignPreviews?.OrderBy(p => p.CreatedAt).Skip(3).FirstOrDefault()?.PreviewImageUrl,
                TemplatePrice = d.CustomShoeDesignTemplate?.Price ?? 0,
                ServicePrice = d.DesignServices?.Sum(ds => ds.Service?.Price ?? 0) ?? 0,
                Total = totalAmount
            });
        }
        return responses;
    }

    private async Task<IEnumerable<GetSimpleCustomShoeDesignResponse>> GetAllPublicCustomShoeDesigns()
    {
        var designs = await _customShoeDesignRepository.GetAllPublicCustomShoeDesignsAsync();
        if (designs == null || !designs.Any())
        {
            return new List<GetSimpleCustomShoeDesignResponse>();
        }
        var responses = new List<GetSimpleCustomShoeDesignResponse>();
        foreach (var d in designs)
        {
            var totalAmount = await CalculateTotalAmount(d);
            responses.Add(new GetSimpleCustomShoeDesignResponse
            {
                Id = d.Id,
                ManufacturerId = d.DesignServices?.FirstOrDefault()?.Service?.ManufacturerId ?? 0,
                Name = d.CustomShoeDesignTemplate?.Name,
                Gender = d.CustomShoeDesignTemplate?.Gender,
                Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
                Status = d.Status,
                RatingCount = d.Ratings?.Count ?? 0,
                PreviewImageUrl = d.DesignPreviews?.OrderBy(p => p.CreatedAt).Skip(3).FirstOrDefault()?.PreviewImageUrl,
                TemplatePrice = d.CustomShoeDesignTemplate?.Price ?? 0,
                ServicePrice = d.DesignServices?.Sum(ds => ds.Service?.Price ?? 0) ?? 0,
                Total = totalAmount
            });
        }
        return responses;
    }

    private async Task<IEnumerable<GetSimpleCustomShoeDesignResponse>> GetTopFiveBestSellingDesigns()
    {
        var designIds = await _orderDetailRepository.GetTopFiveBestSellingDesignsAsync();
        var designs = await _customShoeDesignRepository.GetDesignsByIdsAsync(designIds);
        if (designs == null || !designs.Any())
        {
            return new List<GetSimpleCustomShoeDesignResponse>();
        }
        var responses = new List<GetSimpleCustomShoeDesignResponse>();
        foreach (var d in designs)
        {
            var totalAmount = await CalculateTotalAmount(d);
            responses.Add(new GetSimpleCustomShoeDesignResponse
            {
                Id = d.Id,
                Name = d.CustomShoeDesignTemplate?.Name,
                Gender = d.CustomShoeDesignTemplate?.Gender,
                Rating = d.Ratings != null && d.Ratings.Any() ? (float)Math.Round(d.Ratings.Average(r => r.UserRating), 1) : 0,
                Status = d.Status,
                RatingCount = d.Ratings?.Count ?? 0,
                PreviewImageUrl = d.DesignPreviews?.OrderBy(p => p.CreatedAt).Skip(3).FirstOrDefault()?.PreviewImageUrl,
                TemplatePrice = d.CustomShoeDesignTemplate?.Price ?? 0,
                ServicePrice = d.DesignServices?.Sum(ds => ds.Service?.Price ?? 0) ?? 0,
                Total = totalAmount
            });
        }
        return responses;
    }

    private async Task<GetCustomShoeDesignByIdResponse> GetCustomShoeDesignById(GetCustomShoeDesignByIdRequest request)
    {
        var design = await _customShoeDesignRepository.GetAll()
                                                        .Include(d => d.Ratings)
                                                        .Include(d => d.User)
                                                        .Include(d => d.CustomShoeDesignTemplate)
                                                        .Include(d => d.CustomShoeDesignTextures)
                                                            .ThenInclude(t => t.Texture)
                                                        .Include(d => d.DesignPreviews)
                                                        .Include(d => d.DesignServices)
                                                            .ThenInclude(ds => ds.Service)
                                                        .Where(d => d.IsDeleted == false)
                                                        .FirstOrDefaultAsync(d => d.Id == request.Id);
        var sizes = await _sizeRepository.GetAllAsync();
        // var services = await _serviceRepository.GetActiveServicesAsync();
        if (design == null)
        {
            return null;
        }

        // long manufacturerId = 0;
        // var designServices = await _designServiceRepository.GetAll()
        //     .Include(ds => ds.Service)
        //         .ThenInclude(s => s.Manufacturer)
        //     .Where(ds => ds.CustomShoeDesignId == design.Id)
        //     .ToListAsync();

        // if (designServices != null && designServices.Any())
        // {
        //     var firstService = designServices.First();
        //     if (firstService.Service?.ManufacturerId != null)
        //     {
        //         manufacturerId = firstService.Service.ManufacturerId;
        //     }
        // }

        var totalAmount = await CalculateTotalAmount(design);
        return new GetCustomShoeDesignByIdResponse
        {
            Id = design.Id,
            ManufacturerId = design.DesignServices?.FirstOrDefault()?.Service?.ManufacturerId ?? 0,
            Name = design.CustomShoeDesignTemplate?.Name,
            Description = design.Description,
            Price = totalAmount,
            TemplateUrl = design.CustomShoeDesignTemplate?.ThreeDFileUrl,
            DesignData = design.DesignData,
            Sizes = sizes.Select(d => new DTOs.Size.ShoeSizes
            {
                Id = d.Id,
                SizeValue = d.SizeValue
            }),
            PreviewImages = design.DesignPreviews.Where(d => d.CustomShoeDesignId == design.Id).Select(d => d.PreviewImageUrl),
            TexturesUrls = design.CustomShoeDesignTextures?.Select(t => t.Texture?.ImageUrl),
            Services = design.DesignServices.Select(s => new DTOs.CustomShoeDesign.Services
            {
                Id = s.ServiceId,
                Price = s.Service?.Price ?? 0
            }) ?? new List<DTOs.CustomShoeDesign.Services>()
        };
    }

    private async Task<GetCustomDesignsByUserIdResponse> GetCustomShoeDesignsByUserId(GetCustomShoeDesignsByUserIdRequest request)
    {
        var designs = await _customShoeDesignRepository.GetAll()
                                                        .Include(d => d.Ratings)
                                                        .Include(d => d.User)
                                                        .Include(d => d.CustomShoeDesignTemplate)
                                                        .Include(d => d.CustomShoeDesignTextures)
                                                            .ThenInclude(t => t.Texture)
                                                        .Include(d => d.DesignPreviews)
                                                        .Include(d => d.DesignServices)
                                                            .ThenInclude(s => s.Service)
                                                        .Where(d => d.IsDeleted == false)
                                                        .Where(d => d.UserId == request.UserId)
                                                        .ToListAsync();
        if (designs == null || !designs.Any())
        {
            return new GetCustomDesignsByUserIdResponse
            {
                Designs = new List<CustomDesignShoeGetByUserIdResponse>()
            };
        }
        var responses = new List<CustomDesignShoeGetByUserIdResponse>();
        foreach (var d in designs)
        {
            var totalAmount = await CalculateTotalAmount(d);
            responses.Add(new CustomDesignShoeGetByUserIdResponse
            {
                Id = d.Id,
                ManufacturerId = d.DesignServices?.FirstOrDefault()?.Service?.ManufacturerId ?? 0,
                Name = d.CustomShoeDesignTemplate?.Name,
                PreviewImageUrl = d.DesignPreviews?.OrderBy(p => p.CreatedAt).Skip(3).FirstOrDefault()?.PreviewImageUrl,
                TemplatePrice = d.CustomShoeDesignTemplate?.Price ?? 0,
                ServicePrice = d.DesignServices?.Sum(ds => ds.Service?.Price ?? 0) ?? 0,
                Total = totalAmount,
                CreatedAt = $"{d.CreatedAt:HH:mm:ss dd/MM/yyyy}",
                Status = d.Status
            });
        }
        return new GetCustomDesignsByUserIdResponse
        {
            Designs = responses
        };
    }

    private async Task<CustomShoeDesign> GetCustomShoeDesignFromAddDesignRequest(AddCustomShoeDesignRequest request)
    {
        var template = await _customShoeDesignTemplateRepository.FindAsync(request.CustomShoeDesignTemplateId ?? 0);
        if (template.IsDeleted == true)
        {
            throw new InvalidOperationException("Template not found");
        }

        int servicesPrice = 0;
        if (request.ServiceIds != null && request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var serviceAmount = await _serviceRepository.FindAsync(serviceId);
                if (serviceAmount != null)
                {
                    servicesPrice += serviceAmount.Price;
                }
            }
        }

        DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
        string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm-ss");
        string fileName = $"designData_{formattedDateTime}.json";
        byte[] fileBytes;

        using (var memoryStream = new MemoryStream())
        {
            await request.DesignData.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var designDataPath = await UploadDesignDataToAzureStorage(fileName, fileBytes);

        var design = new CustomShoeDesign
        {
            UserId = request.UserId ?? 0,
            CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId ?? 0,
            DesignData = designDataPath,
            Name = request.Name,
            Description = request.Description,
            Status = Common.Enums.CustomShoeDesignStatus.Private,
            DesignerMarkup = request.DesignerMarkup ?? 0,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return design;
    }

    private async Task<int> CalculateTotalAmount(CustomShoeDesign design)
    {
        var template = await _customShoeDesignTemplateRepository.FindAsync(design.CustomShoeDesignTemplateId);
        var templatePrice = template?.Price ?? 0;

        int servicesPrice = 0;
        if (design.DesignServices != null && design.DesignServices.Any())
        {
            servicesPrice = design.DesignServices.Sum(ds => ds.Service?.Price ?? 0);
        }

        return templatePrice + servicesPrice + design.DesignerMarkup;
    }

    private async Task<IEnumerable<DesignPreview>> GetDesignPreviewImagesFromAddDesignRequest(AddCustomShoeDesignRequest request, long designId)
    {
        if (request.CustomShoeDesignPreviewImages == null || !request.CustomShoeDesignPreviewImages.Any())
        {
            return new List<DesignPreview>();
        }
        List<DesignPreview> previewImages = new List<DesignPreview>();
        foreach (var previewImage in request.CustomShoeDesignPreviewImages)
        {
            DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
            string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm-ss");
            string fileName = $"previewImage_{Guid.NewGuid()}_{formattedDateTime}.jpeg";
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await previewImage.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var previewImagePath = await UploadImageToAzureStorage(fileName, fileBytes);
            previewImages.Add(new DesignPreview
            {
                CustomShoeDesignId = designId,
                PreviewImageUrl = previewImagePath,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        return previewImages;
    }

    private IEnumerable<CustomShoeDesignTextures> GetDesignTexturesFromAddDesignRequest(AddCustomShoeDesignRequest request, long designId)
    {
        if (request.TextureIds == null || !request.TextureIds.Any())
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
        if (request.ServiceIds == null || !request.ServiceIds.Any())
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

    private async Task<CustomShoeDesign> GetCustomShoeDesignFromUpdateDesignStatusRequest(UpdateCustomShoeDesignStatusRequest request)
    {
        var design = await _customShoeDesignRepository.FindAsync(request.Id);
        if (design == null)
        {
            throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
        }

        if (design.Status == request.Status)
        {
            throw new InvalidOperationException($"Custom shoe design with ID {request.Id} is already {request.Status}");
        }

        design.Status = request.Status;
        design.UpdatedAt = DateTime.UtcNow;
        return design;
    }

    private async Task<CustomShoeDesign> GetCustomShoeDesignFromUpdateDesignRequest(UpdateCustomShoeDesignRequest request)
    {
        var design = await _customShoeDesignRepository.FindAsync((object)request.Id);
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
        var design = await _customShoeDesignRepository.FindAsync((object)request.Id);

        if (design == null)
        {
            throw new InvalidOperationException($"Custom shoe design with ID {request.Id} not found");
        }

        design.IsDeleted = true;
        design.UpdatedAt = DateTime.UtcNow;
        design.Status = Common.Enums.CustomShoeDesignStatus.Archived;

        return design;
    }

    private async Task<string> UploadImageToAzureStorage(string fileName, byte[] fileBytes)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureImageContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            // Determine content type by checking file signature (magic numbers)
            string contentType;
            if (fileBytes.Length >= 2)
            {
                // Check for PNG signature
                if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50)
                {
                    contentType = "image/png";
                }
                // Check for JPEG signature
                else if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
                {
                    contentType = "image/jpeg";
                }
                else
                {
                    throw new InvalidOperationException("Unsupported image format. Only PNG and JPEG are supported.");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid image file: File is too small or empty.");
            }

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = contentType
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
    }
    private async Task<string> UploadDesignDataToAzureStorage(string fileName, byte[] fileBytes)
    {
        try
        {
            // Check JSON file signature
            if (fileBytes.Length == 0)
            {
                throw new InvalidOperationException("File is empty.");
            }

            // JSON files start with either '{' (0x7B) or '[' (0x5B) in UTF-8
            if (fileBytes[0] != 0x7B && fileBytes[0] != 0x5B)
            {
                throw new InvalidOperationException("Invalid JSON file format. File must start with '{' or '['.");
            }

            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureJsonContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = "application/json"
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
    }
    #endregion
}
