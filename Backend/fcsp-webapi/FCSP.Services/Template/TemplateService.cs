using Azure.Core;
using FCSP.DTOs;
using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Models.Entities;
using FCSP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.Services.Template
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateService(ITemplateRepository templateRepository)
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
                Name = customShoeDesignTemplate.Name,
                Description = customShoeDesignTemplate.Description,
                Price = customShoeDesignTemplate.Price,
                ImageUrl = customShoeDesignTemplate.ImageUrl,
            };
        }

        public async Task<AddTemplateResponse> AddTemplate(AddTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromAddRequest(request);
            await _templateRepository.AddAsync(customShoeDesignTemplate);
            return new AddTemplateResponse();
        }

        public async Task<AddTemplateResponse> UpdateTemplate(UpdateTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromUpdateRequest(request);
            await _templateRepository.UpdateAsync(customShoeDesignTemplate);
            return new AddTemplateResponse();
        }

        public async Task<AddTemplateResponse> DeleteTemplate(DeleteTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = GetEntityFromDeleteRequest(request);
            await _templateRepository.DeleteAsync(customShoeDesignTemplate.Id);
            return new AddTemplateResponse();
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
                Price = request.Price,
                ImageUrl = request.ImageUrl,
            };
        }

        private CustomShoeDesignTemplate GetEntityFromUpdateRequest(UpdateTemplateRequest request)
        {
            CustomShoeDesignTemplate customShoeDesignTemplate = _templateRepository.Find(request.Id);
            if (customShoeDesignTemplate == null)
            {
                throw new InvalidOperationException();
            }
            customShoeDesignTemplate.Name = request.Name ?? customShoeDesignTemplate.Name;
            customShoeDesignTemplate.Description = request.Description ?? customShoeDesignTemplate.Description;
            customShoeDesignTemplate.Price = request.Price ?? customShoeDesignTemplate.Price;
            customShoeDesignTemplate.ImageUrl = request.ImageUrl ?? customShoeDesignTemplate.ImageUrl;
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
