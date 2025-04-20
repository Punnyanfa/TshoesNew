using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FCSP.Services.TemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ICustomShoeDesignTemplateRepository _templateRepository;
        private readonly IConfiguration _configuration;
        private readonly string? _azureConnectionString;
        private readonly string? _azureContainerName;

        public TemplateService(ICustomShoeDesignTemplateRepository templateRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _templateRepository = templateRepository;
            _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
            _azureContainerName = _configuration["AzureStorage:ContainerName"];
        }

        private BaseResponseModel<T> HandleException<T>(Exception ex, string action) =>
            new BaseResponseModel<T> { Code = 500, Message = $"Error {action}: {ex.Message}"};

        public async Task<BaseResponseModel<List<CustomShoeDesignTemplate>>> GetAllTemplate()
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                return new BaseResponseModel<List<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Templates retrieved successfully",
                    Data = templates.ToList()
                };
            }
            catch (Exception ex) { return HandleException<List<CustomShoeDesignTemplate>>(ex, "retrieving templates"); }
        }

        public async Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request)
        {
            try
            {
                var template = await _templateRepository.FindAsync(request.Id);
                if (template == null)
                    return new BaseResponseModel<GetTemplateByIdResponse> { Code = 404, Message = "Template not found" };

                return new BaseResponseModel<GetTemplateByIdResponse>
                {
                    Code = 200,
                    Message = "Template retrieved successfully",
                    Data = new GetTemplateByIdResponse
                    {
                        Id = template.Id,
                        Name = template.Name,
                        Description = template.Description ?? string.Empty,
                        Gender = template.Gender ?? string.Empty,
                        Color = template.Color ?? string.Empty,
                        PreviewImageUrl = template.TwoDImageUrl ?? string.Empty,
                        Model3DUrl = template.ThreeDFileUrl ?? string.Empty,
                        BasePrice = (decimal)template.Price,
                        IsAvailable = !template.IsDeleted,
                        CreatedAt = template.CreatedAt,
                        UpdatedAt = template.UpdatedAt
                    }
                };
            }
            catch (Exception ex) { return HandleException<GetTemplateByIdResponse>(ex, "retrieving template"); }
        }

        public async Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Name))
                    return new BaseResponseModel<AddTemplateResponse> { Code = 400, Message = "Name is required" };

                if (request.BasePrice < 0)
                    return new BaseResponseModel<AddTemplateResponse> { Code = 400, Message = "BasePrice cannot be negative" };

                var template = await GetEntityFromAddRequest(request);
                await _templateRepository.AddAsync(template);

                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 201,
                    Message = "Template created successfully",
                    Data = new AddTemplateResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex) { return HandleException<AddTemplateResponse>(ex, "creating template"); }
        }

        public async Task<BaseResponseModel<UpdateTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                    return new BaseResponseModel<UpdateTemplateResponse> { Code = 404, Message = "Template not found" };

                var previewImageUrl = await UploadPreviewImage(request.PreviewImage);
                var model3DUrl = await Upload3DModel(request.Model3DFile);

                template.Name = request.Name ?? template.Name;
                template.Description = request.Description ?? template.Description;
                template.Gender = request.Gender ?? template.Gender;
                template.Color = request.Color ?? template.Color;
                template.Price = (float)request.BasePrice;
                template.TwoDImageUrl = previewImageUrl;
                template.ThreeDFileUrl = model3DUrl;
                template.UpdatedAt = DateTime.Now;

                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<UpdateTemplateResponse>
                {
                    Code = 200,
                    Message = "Template updated successfully",
                    Data = new UpdateTemplateResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateTemplateResponse>
                {
                    Code = 500,
                    Message = $"Error updating template: {ex.Message}"
                    
                };
            }
        }

        public async Task<BaseResponseModel<DeleteTemplateResponse>> DeleteTemplate(DeleteTemplateRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                    return new BaseResponseModel<DeleteTemplateResponse> { Code = 404, Message = "Template not found" };

                template.IsDeleted = true;
                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<DeleteTemplateResponse>
                {
                    Code = 200,
                    Message = "Template deleted successfully",
                    Data = new DeleteTemplateResponse { Success = true }
                };
            }
            catch (Exception ex) { return HandleException<DeleteTemplateResponse>(ex, "deleting template"); }
        }

        public async Task<BaseResponseModel<IEnumerable<long>>> GetCustomShoeDesignIdsByTemplate(long templateId)
        {
            try
            {
                var template = _templateRepository.Find(templateId);
                if (template == null)
                    return new BaseResponseModel<IEnumerable<long>> { Code = 404, Message = "Template not found" };

                var designIds = await _templateRepository.GetCustomShoeDesignIdsByTemplateAsync(templateId);
                return new BaseResponseModel<IEnumerable<long>>
                {
                    Code = 200,
                    Message = "Design IDs retrieved successfully",
                    Data = designIds
                };
            }
            catch (Exception ex) { return HandleException<IEnumerable<long>>(ex, "retrieving design IDs"); }
        }

        public async Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                var availableTemplates = templates.Where(t => !t.IsDeleted).ToList();
                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Available templates retrieved successfully",
                    Data = availableTemplates
                };
            }
            catch (Exception ex) { return HandleException<IEnumerable<CustomShoeDesignTemplate>>(ex, "retrieving available templates"); }
        }

        public async Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> SearchTemplates(SearchTemplatesRequest request)
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                var filteredTemplates = templates.Where(t =>
                    (!t.IsDeleted || request.IsAvailable == false) &&
                    (string.IsNullOrEmpty(request.Name) || t.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(request.Gender) || t.Gender == request.Gender) &&
                    (string.IsNullOrEmpty(request.Color) || t.Color == request.Color) &&
                    (!request.MinPrice.HasValue || (decimal)t.Price >= request.MinPrice.Value) && 
                    (!request.MaxPrice.HasValue || (decimal)t.Price <= request.MaxPrice.Value)
                ).ToList();

                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Templates retrieved successfully",
                    Data = filteredTemplates
                };
            }
            catch (Exception ex) { return HandleException<IEnumerable<CustomShoeDesignTemplate>>(ex, "searching templates"); }
        }

        public async Task<BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>> GetPopularTemplates(GetPopularTemplatesRequest request)
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                var popularTemplates = templates
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.CustomShoeDesigns.Count)
                    .Take(request.Limit)
                    .Select(t => new GetPopularTemplatesResponse
                    {
                        Id = t.Id,
                        Name = t.Name,
                        PreviewImageUrl = t.TwoDImageUrl ?? string.Empty,
                        BasePrice = (decimal)t.Price
                    }).ToList();

                return new BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>
                {
                    Code = 200,
                    Message = "Popular templates retrieved successfully",
                    Data = popularTemplates
                };
            }
            catch (Exception ex) { return HandleException<IEnumerable<GetPopularTemplatesResponse>>(ex, "retrieving popular templates"); }
        }

        public async Task<BaseResponseModel<UpdateTemplateStatusResponse>> UpdateTemplateStatus(UpdateTemplateStatusRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                    return new BaseResponseModel<UpdateTemplateStatusResponse> { Code = 404, Message = "Template not found" };

                template.Status = request.Status;
                template.UpdatedAt = DateTime.Now;
                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<UpdateTemplateStatusResponse>
                {
                    Code = 200,
                    Message = "Template restored successfully",
                    Data = new UpdateTemplateStatusResponse
                    {    
                        Success = true
                    }
                };
            }
            catch (Exception ex) { return HandleException<UpdateTemplateStatusResponse>(ex, "restoring template"); }
        }

        public async Task<BaseResponseModel<GetTemplateStatsResponse>> GetTemplateStats(GetTemplateStatsRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                    return new BaseResponseModel<GetTemplateStatsResponse> { Code = 404, Message = "Template not found" };

                return new BaseResponseModel<GetTemplateStatsResponse>
                {
                    Code = 200,
                    Message = "Template stats retrieved successfully",
                    Data = new GetTemplateStatsResponse
                    {
                        TemplateId = template.Id,
                        Name = template.Name,
                        CustomShoeDesignCount = await _templateRepository.GetCustomShoeDesignCountByTemplateAsync(template.Id),
                        CreatedAt = template.CreatedAt,
                        LastUpdated = template.UpdatedAt,
                        IsAvailable = !template.IsDeleted
                    }
                };
            }
            catch (Exception ex) { return HandleException<GetTemplateStatsResponse>(ex, "retrieving template stats"); }
        }

        private async Task<CustomShoeDesignTemplate> GetEntityFromAddRequest(AddTemplateRequest request) 
        {
            var previewImageUrl = await UploadPreviewImage(request.PreviewImage);
            var model3DUrl = await Upload3DModel(request.Model3DFile);

            return new CustomShoeDesignTemplate
            {
                Name = request.Name,
                Description = request.Description,
                Gender = request.Gender,
                Color = request.Color,
                Price = (float)request.BasePrice,
                TwoDImageUrl = previewImageUrl,
                ThreeDFileUrl = model3DUrl,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                Status = Common.Enums.TemplateStatus.Private
            };
        }

        private async Task<string> UploadPreviewImage(IFormFile previewImage)
        {
            DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
            string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
            string fileName = $"templatePreviewImage_{formattedDateTime}.jpeg";
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await previewImage.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var avatarPath = await UploadToAzureStorage(fileName, fileBytes);

            return avatarPath;
        }

        private async Task<string> Upload3DModel(IFormFile model3DFile)
        {
            DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
            string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
            string fileName = $"template3DModel_{formattedDateTime}.glb";
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await model3DFile.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var avatarPath = await UploadToAzureStorage(fileName, fileBytes);

            return avatarPath;
        }

        private async Task<string> UploadToAzureStorage(string fileName, byte[] fileBytes)
        {
        try
        {
            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = "model/gltf-binary"
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
        }
    }
}