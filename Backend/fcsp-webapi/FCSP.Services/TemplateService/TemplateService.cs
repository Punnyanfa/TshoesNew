using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

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
    }
}
