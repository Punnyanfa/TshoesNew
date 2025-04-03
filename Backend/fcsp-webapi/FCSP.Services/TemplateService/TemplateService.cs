using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.TemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ICustomShoeDesignTemplateRepository _templateRepository;

        public TemplateService(ICustomShoeDesignTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        #region Public Methods
        public async Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAllTemplate()
        {
            try
            {
                var response = await _templateRepository.GetAllAsync();
                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Templates retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request)
        {
            try
            {
                CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromGetByIdRequest(request);
                var response = new GetTemplateByIdResponse
                {
                    Id = customShoeDesignTemplate.Id,
                    Name = customShoeDesignTemplate.Name,
                    Description = customShoeDesignTemplate.Description ?? string.Empty,
                    PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty,
                    Model3DUrl = customShoeDesignTemplate.ThreeDFileUrl ?? string.Empty,
                    BasePrice = (decimal)customShoeDesignTemplate.Price,
                    IsAvailable = !customShoeDesignTemplate.IsDeleted,
                    CreatedAt = customShoeDesignTemplate.CreatedAt,
                    UpdatedAt = customShoeDesignTemplate.UpdatedAt
                };

                return new BaseResponseModel<GetTemplateByIdResponse>
                {
                    Code = 200,
                    Message = "Template retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetTemplateByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request)
        {
            try
            {
                CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromAddRequest(request);
                await _templateRepository.AddAsync(customShoeDesignTemplate);

                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 200,
                    Message = "Template added successfully",
                    Data = new AddTemplateResponse
                    {
                        Id = customShoeDesignTemplate.Id,
                        Name = customShoeDesignTemplate.Name,
                        PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request)
        {
            try
            {
                CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromUpdateRequest(request);
                await _templateRepository.UpdateAsync(customShoeDesignTemplate);

                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 200,
                    Message = "Template updated successfully",
                    Data = new AddTemplateResponse
                    {
                        Id = customShoeDesignTemplate.Id,
                        Name = customShoeDesignTemplate.Name,
                        PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteTemplateResponse>> DeleteTemplate(DeleteTemplateRequest request)
        {
            try
            {
                CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromDeleteRequest(request);
                customShoeDesignTemplate.IsDeleted = true; // Soft delete thay vì xóa hẳn
                await _templateRepository.UpdateAsync(customShoeDesignTemplate); // Sử dụng Update thay vì Delete

                return new BaseResponseModel<DeleteTemplateResponse>
                {
                    Code = 200,
                    Message = "Template deleted successfully",
                    Data = new DeleteTemplateResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteTemplateResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new DeleteTemplateResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<IEnumerable<long>>> GetCustomShoeDesignIdsByTemplate(long templateId)
        {
            try
            {
                var template = await _templateRepository.FindAsync(templateId);
                if (template == null)
                {
                    throw new InvalidOperationException($"Template with ID {templateId} not found");
                }

                var designIds = template.CustomShoeDesigns.Select(design => design.Id);

                return new BaseResponseModel<IEnumerable<long>>
                {
                    Code = 200,
                    Message = "Custom shoe design IDs retrieved successfully",
                    Data = designIds
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<long>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                var availableTemplates = templates.Where(t => !t.IsDeleted);

                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Available templates retrieved successfully",
                    Data = availableTemplates
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>> SearchTemplates(SearchTemplatesRequest request)
        {
            try
            {
                var templates = _templateRepository.GetAll();

                if (!string.IsNullOrEmpty(request.Name))
                    templates = templates.Where(t => t.Name.Contains(request.Name));
                if (!string.IsNullOrEmpty(request.Gender))
                    templates = templates.Where(t => t.Gender == request.Gender);
                if (!string.IsNullOrEmpty(request.Color))
                    templates = templates.Where(t => t.Color == request.Color);
                if (request.MinPrice.HasValue)
                    templates = templates.Where(t => t.Price >= (float)request.MinPrice.Value);
                if (request.MaxPrice.HasValue)
                    templates = templates.Where(t => t.Price <= (float)request.MaxPrice.Value);
                if (request.IsAvailable.HasValue)
                    templates = templates.Where(t => t.IsDeleted == !request.IsAvailable.Value);

                var result = await templates.ToListAsync();

                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 200,
                    Message = "Templates retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<CustomShoeDesignTemplate>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
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
                        CustomShoeDesignCount = t.CustomShoeDesigns.Count
                    });

                return new BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>
                {
                    Code = 200,
                    Message = "Popular templates retrieved successfully",
                    Data = popularTemplates
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<GetPopularTemplatesResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<RestoreTemplateResponse>> RestoreTemplate(RestoreTemplateRequest request)
        {
            try
            {
                var template = await _templateRepository.FindAsync(request.Id);
                if (template == null)
                {
                    return new BaseResponseModel<RestoreTemplateResponse>
                    {
                        Code = 404,
                        Message = "Template not found",
                        Data = new RestoreTemplateResponse
                        {
                            Id = request.Id,
                            Success = false,
                            Message = "Template not found"
                        }
                    };
                }

                if (!template.IsDeleted)
                {
                    return new BaseResponseModel<RestoreTemplateResponse>
                    {
                        Code = 400,
                        Message = "Template is already active",
                        Data = new RestoreTemplateResponse
                        {
                            Id = request.Id,
                            Success = false,
                            Message = "Template is already active"
                        }
                    };
                }

                template.IsDeleted = false;
                template.UpdatedAt = DateTime.Now;
                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<RestoreTemplateResponse>
                {
                    Code = 200,
                    Message = "Template restored successfully",
                    Data = new RestoreTemplateResponse
                    {
                        Id = template.Id,
                        Success = true,
                        Message = "Template restored successfully"
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<RestoreTemplateResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new RestoreTemplateResponse
                    {
                        Id = request.Id,
                        Success = false,
                        Message = ex.Message
                    }
                };
            }
        }

        public async Task<BaseResponseModel<GetTemplateStatsResponse>> GetTemplateStats(GetTemplateStatsRequest request)
        {
            try
            {
                var template = await _templateRepository.FindAsync(request.Id);
                if (template == null)
                {
                    throw new InvalidOperationException($"Template with ID {request.Id} not found");
                }

                var stats = new GetTemplateStatsResponse
                {
                    Id = template.Id,
                    Name = template.Name,
                    CustomShoeDesignCount = template.CustomShoeDesigns.Count,
                    CreatedAt = template.CreatedAt,
                    LastUpdatedAt = template.UpdatedAt,
                    IsAvailable = !template.IsDeleted
                };

                return new BaseResponseModel<GetTemplateStatsResponse>
                {
                    Code = 200,
                    Message = "Template statistics retrieved successfully",
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetTemplateStatsResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        #endregion

        #region Private Methods
        private CustomShoeDesignTemplate GetEntityFromGetByIdRequest(GetTemplateByIdRequest request)
        {
            CustomShoeDesignTemplate template = _templateRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException($"Template with ID {request.Id} not found");
            }
            return template;
        }

        private CustomShoeDesignTemplate GetEntityFromAddRequest(AddTemplateRequest request)
        {
            return new CustomShoeDesignTemplate
            {
                Name = request.Name,
                Description = request.Description,
                Gender = request.Gender,
                Color = request.Color,
                Price = (float)request.BasePrice,
                TwoDImageUrl = request.PreviewImageUrl,
                ThreeDFileUrl = request.Model3DUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        private CustomShoeDesignTemplate GetEntityFromUpdateRequest(UpdateTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = _templateRepository.Find(request.Id);

            if (customShoeDesignTemplate == null)
            {
                throw new InvalidOperationException($"Template with ID {request.Id} not found");
            }
            customShoeDesignTemplate.Name = request.Name;
            customShoeDesignTemplate.Description = request.Description;
            customShoeDesignTemplate.Gender = request.Gender;
            customShoeDesignTemplate.Color = request.Color;
            customShoeDesignTemplate.Price = (float)request.BasePrice;
            customShoeDesignTemplate.TwoDImageUrl = request.PreviewImageUrl;
            customShoeDesignTemplate.ThreeDFileUrl = request.Model3DUrl;
            customShoeDesignTemplate.UpdatedAt = DateTime.UtcNow;

            return customShoeDesignTemplate;
        }

        private CustomShoeDesignTemplate GetEntityFromDeleteRequest(DeleteTemplateRequest request)
        {
            CustomShoeDesignTemplate template = _templateRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException($"Template with ID {request.Id} not found");
            }
            return template;
        }
        #endregion
    }
}