using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Services.TemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ICustomShoeDesignTemplateRepository _templateRepository;

        public TemplateService(ICustomShoeDesignTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<IEnumerable<CustomShoeDesignTemplate>> GetAllTemplate()
        {
            var response = await _templateRepository.GetAllAsync();
            return response;
        }

        public async Task<GetTemplateByIdResponse> GetTemplateById(GetTemplateByIdRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromGetByIdRequest(request);

            return new GetTemplateByIdResponse
            {
                Id = customShoeDesignTemplate.Id,
                Name = customShoeDesignTemplate.Name,
                Description = customShoeDesignTemplate.Description ?? string.Empty,
                PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty,
                Model3DUrl = customShoeDesignTemplate.ThreeDFileUrl ?? string.Empty,
                BasePrice = (decimal)customShoeDesignTemplate.Price,
                IsAvailable = true,
                CreatedAt = customShoeDesignTemplate.CreatedAt,
                UpdatedAt = customShoeDesignTemplate.UpdatedAt
            };
        }

        public async Task<AddTemplateResponse> AddTemplate(AddTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromAddRequest(request);
            await _templateRepository.AddAsync(customShoeDesignTemplate);
            return new AddTemplateResponse
            {
                Id = customShoeDesignTemplate.Id,
                Name = customShoeDesignTemplate.Name,
                PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty
            };
        }

        public async Task<AddTemplateResponse> UpdateTemplate(UpdateTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromUpdateRequest(request);

            await _templateRepository.UpdateAsync(customShoeDesignTemplate);
            return new AddTemplateResponse
            {
                Id = customShoeDesignTemplate.Id,
                Name = customShoeDesignTemplate.Name,
                PreviewImageUrl = customShoeDesignTemplate.TwoDImageUrl ?? string.Empty
            };
        }

        public async Task<DeleteTemplateResponse> DeleteTemplate(DeleteTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromDeleteRequest(request);

            await _templateRepository.DeleteAsync(customShoeDesignTemplate.Id);
            return new DeleteTemplateResponse
            {
                Success = true
            };
        }

        private CustomShoeDesignTemplate GetEntityFromGetByIdRequest(GetTemplateByIdRequest request)
        {
            CustomShoeDesignTemplate template = _templateRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
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
                ThreeDFileUrl = request.Model3DUrl
            };
        }

        private CustomShoeDesignTemplate GetEntityFromUpdateRequest(UpdateTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = _templateRepository.Find(request.Id);

            if (customShoeDesignTemplate == null)
            {
                throw new InvalidOperationException();
            }
            customShoeDesignTemplate.Name = request.Name;
            customShoeDesignTemplate.Description = request.Description;
            customShoeDesignTemplate.Gender = request.Gender;
            customShoeDesignTemplate.Color = request.Color;
            customShoeDesignTemplate.Price = (float)request.BasePrice;
            customShoeDesignTemplate.TwoDImageUrl = request.PreviewImageUrl;
            customShoeDesignTemplate.ThreeDFileUrl = request.Model3DUrl;

            customShoeDesignTemplate.UpdatedAt = DateTime.Now;
            return customShoeDesignTemplate;
        }

        private CustomShoeDesignTemplate GetEntityFromDeleteRequest(DeleteTemplateRequest request)
        {
            CustomShoeDesignTemplate template = _templateRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
            }
            return template;
        }
        public async Task<IEnumerable<long>> GetCustomShoeDesignIdsByTemplate(long templateId)
        {
            var template = await _templateRepository.FindAsync(templateId);
            if (template == null)
            {
                throw new InvalidOperationException($"Template with ID {templateId} not found");
            }

            return template.CustomShoeDesigns.Select(design => design.Id);
        }

        public async Task<IEnumerable<CustomShoeDesignTemplate>> GetAvailableTemplates()
        {
            var templates = await _templateRepository.GetAllAsync();
            return templates.Where(t => !t.IsDeleted);
        }
        public async Task<IEnumerable<CustomShoeDesignTemplate>> SearchTemplates(SearchTemplatesRequest request)
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

            return await templates.ToListAsync();
        }
        public async Task<IEnumerable<GetPopularTemplatesResponse>> GetPopularTemplates(GetPopularTemplatesRequest request)
        {
            var templates = await _templateRepository.GetAllAsync();
            var popularTemplates = templates
                .Where(t => !t.IsDeleted) // Chỉ lấy các template available
                .OrderByDescending(t => t.CustomShoeDesigns.Count) // Sắp xếp theo số lượng CustomShoeDesign
                .Take(request.Limit) // Giới hạn số lượng
                .Select(t => new GetPopularTemplatesResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    PreviewImageUrl = t.TwoDImageUrl ?? string.Empty,
                    CustomShoeDesignCount = t.CustomShoeDesigns.Count
                });

            return popularTemplates;
        }

        public async Task<RestoreTemplateResponse> RestoreTemplate(RestoreTemplateRequest request)
        {
            var template = await _templateRepository.FindAsync(request.Id);
            if (template == null)
            {
                return new RestoreTemplateResponse
                {
                    Id = request.Id,
                    Success = false,
                    Message = "Template not found"
                };
            }

            if (!template.IsDeleted)
            {
                return new RestoreTemplateResponse
                {
                    Id = request.Id,
                    Success = false,
                    Message = "Template is already active"
                };
            }

            template.IsDeleted = false;
            template.UpdatedAt = DateTime.Now;
            await _templateRepository.UpdateAsync(template);

            return new RestoreTemplateResponse
            {
                Id = template.Id,
                Success = true,
                Message = "Template restored successfully"
            };
        }

        public async Task<GetTemplateStatsResponse> GetTemplateStats(GetTemplateStatsRequest request)
        {
            var template = await _templateRepository.FindAsync(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException($"Template with ID {request.Id} not found");
            }

            return new GetTemplateStatsResponse
            {
                Id = template.Id,
                Name = template.Name,
                CustomShoeDesignCount = template.CustomShoeDesigns.Count,
                CreatedAt = template.CreatedAt,
                LastUpdatedAt = template.UpdatedAt,
                IsAvailable = !template.IsDeleted
            };
        }
    }
}
