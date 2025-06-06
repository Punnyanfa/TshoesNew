using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using FCSP.Common.Utils;

namespace FCSP.Services.TemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ICustomShoeDesignTemplateRepository _templateRepository;
        private readonly IConfiguration _configuration;
        private readonly string? _azureConnectionString;
        private readonly string? _imagesContainer;
        private readonly string? _3dmodelsContainer;

        public TemplateService(ICustomShoeDesignTemplateRepository templateRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _templateRepository = templateRepository;
            _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
            _3dmodelsContainer = _configuration["AzureStorage:3DModelsContainer"];
            _imagesContainer = _configuration["AzureStorage:ImagesContainer"];
        }

        private BaseResponseModel<T> HandleException<T>(Exception ex, string action) =>
            new BaseResponseModel<T> { Code = 500, Message = $"Error {action}: {ex.Message}" };

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
                string previewImageUrl = null;
                string model3DUrl = null;
                if (request.PreviewImage != null)
                {
                    previewImageUrl = await UploadPreviewImage(request.PreviewImage);
                }
                if (request.Model3DFile != null)
                {
                    model3DUrl = await Upload3DModel(request.Model3DFile);
                }

                template.Name = request.Name ?? template.Name;
                template.Description = request.Description ?? template.Description;
                template.Gender = request.Gender ?? template.Gender;
                template.Color = request.Color ?? template.Color;
                template.Price = request.BasePrice ?? template.Price;
                template.TwoDImageUrl = previewImageUrl ?? template.TwoDImageUrl;
                template.ThreeDFileUrl = model3DUrl;
                template.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();

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
                var template = await _templateRepository.FindAsync(request.Id);
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
                var template = await _templateRepository.FindAsync(request.Id); 
                if (template == null)
                    return new BaseResponseModel<UpdateTemplateStatusResponse> { Code = 404, Message = "Template not found" };

                if (!Enum.IsDefined(typeof(TemplateStatus), request.Status))
                    return new BaseResponseModel<UpdateTemplateStatusResponse> { Code = 400, Message = "status is invalid" };

                template.Status = request.Status;
                template.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
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
          
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidOperationException("Name is required");
            if (request.Name.Length < 5)
                throw new InvalidOperationException("Name must be greater than 5 characters");
            if (request.Name.Length > 50)
                throw new InvalidOperationException("Name must be less than 50 characters" );
            if (!Regex.IsMatch(request.Name, @"^[a-zA-Z0-9\s]+$"))
                throw new InvalidOperationException("Invalid name format" );

            // Description validations
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new InvalidOperationException("Description is require" );
            var wordCount = request.Description.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            if (wordCount < 3)
                throw new InvalidOperationException("Description must be greater than 3 words" );
            if (wordCount > 25)
                throw new InvalidOperationException("Description must be less than 25 words" );
            if (!Regex.IsMatch(request.Description, @"^[a-zA-Z0-9\s.,]+$"))
                throw new InvalidOperationException("Invalid description format" );

            // Gender validations
            if (string.IsNullOrWhiteSpace(request.Gender))
                 throw new InvalidOperationException("Gender is require" );
            if (request.Gender != "Male" && request.Gender != "Female" && request.Gender != "Other")
                throw new InvalidOperationException("Gender is invalid" );

            // Color validations
            if (string.IsNullOrWhiteSpace(request.Color))
                throw new InvalidOperationException("Color is require" );
            if (!Regex.IsMatch(request.Color, @"^[a-zA-Z\s,]+$"))
                throw new InvalidOperationException("Color is invalid" );

            // PreviewImage validations
            if (request.PreviewImage == null)
                throw new InvalidOperationException("Preview Image is require" );
            // File format validation is handled in UploadPreviewImage

            // Model3DFile validations
            if (request.Model3DFile == null)
                throw new InvalidOperationException("Model3DFile is require" );
            // File format validation is handled in Upload3DModel

            // BasePrice validations
            if (request.BasePrice <= 0)
                throw new InvalidOperationException("Invalid BasePrice format" );
            if (request.BasePrice < 0)
                throw new InvalidOperationException("BasePrice cannot be negative" );

            // IsAvailable validations
            if (!request.IsAvailable)
                throw new InvalidOperationException("isAvalaible is require" );
            var previewImageUrl = await UploadPreviewImage(request.PreviewImage);
            var model3DUrl = await Upload3DModel(request.Model3DFile);

            return new CustomShoeDesignTemplate
            {
                Name = request.Name,
                Description = request.Description,
                Gender = request.Gender,
                Color = request.Color,
                Price = (int)request.BasePrice,
                TwoDImageUrl = previewImageUrl,
                ThreeDFileUrl = model3DUrl,
                CreatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                IsDeleted = false,
                Status = Common.Enums.TemplateStatus.Private
            };
        }

        private async Task<string> UploadPreviewImage(IFormFile previewImage)
        {
            DateTime gmtPlus7Time = DateTimeUtils.GetCurrentGmtPlus7();
            string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
            string fileName = $"templatePreviewImage_{formattedDateTime}.jpeg";
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await previewImage.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var avatarPath = await UploadPreviewImageToAzureStorage(fileName, fileBytes);

            return avatarPath;
        }

        private async Task<string> Upload3DModel(IFormFile model3DFile)
        {
            DateTime gmtPlus7Time = DateTimeUtils.GetCurrentGmtPlus7();
            string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
            string fileName = $"template3DModel_{formattedDateTime}.glb";
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await model3DFile.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var avatarPath = await Upload3DModelToAzureStorage(fileName, fileBytes);

            return avatarPath;
        }

        private async Task<string> Upload3DModelToAzureStorage(string fileName, byte[] fileBytes)
        {
            try
            {
                // Check GLB file signature
                if (fileBytes.Length < 12)
                {
                    throw new InvalidOperationException("File is too small to be a valid GLB file.");
                }

                // GLB files start with "glTF" (0x67 0x6C 0x54 0x46) followed by version and length
                if (fileBytes[0] != 0x67 || fileBytes[1] != 0x6C || fileBytes[2] != 0x54 || fileBytes[3] != 0x46)
                {
                    throw new InvalidOperationException("Invalid GLB file format. File must be a valid GLB binary file.");
                }

                var blobServiceClient = new BlobServiceClient(_azureConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(_3dmodelsContainer);
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

        private async Task<string> UploadPreviewImageToAzureStorage(string fileName, byte[] fileBytes)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_azureConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(_imagesContainer);
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
    }
}