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
                var template = _templateRepository.Find(request.Id);
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
                        Gender = template.Gender,
                        Color = template.Color,
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

                var template = GetEntityFromAddRequest(request);
                await _templateRepository.AddAsync(template);

                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 201,
                    Message = "Template created successfully",
                    Data = new AddTemplateResponse
                    {
                        Id = template.Id,
                        Name = template.Name,
                        PreviewImageUrl = template.TwoDImageUrl ?? string.Empty
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

                template.Name = request.Name;
                template.Description = request.Description;
                template.Gender = request.Gender;
                template.Color = request.Color;
                template.Price = (float)request.BasePrice;
                template.TwoDImageUrl = request.PreviewImageUrl;
                template.ThreeDFileUrl = request.Model3DUrl;
                template.UpdatedAt = DateTime.Now;

                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<UpdateTemplateResponse>
                {
                    Code = 200,
                    Message = "Template updated successfully",
                    Data = new UpdateTemplateResponse
                    {
                        Id = template.Id,
                        Name = template.Name,
                        PreviewImageUrl = template.TwoDImageUrl ?? string.Empty
                    }
                };
            }
            catch (Exception ex) { return HandleException<UpdateTemplateResponse>(ex, "updating template"); }
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

        public async Task<BaseResponseModel<RestoreTemplateResponse>> RestoreTemplate(RestoreTemplateRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                    return new BaseResponseModel<RestoreTemplateResponse> { Code = 404, Message = "Template not found" };

                if (!template.IsDeleted)
                    return new BaseResponseModel<RestoreTemplateResponse> { Code = 400, Message = "Template is already active" };

                template.IsDeleted = false;
                template.UpdatedAt = DateTime.Now;
                await _templateRepository.UpdateAsync(template);

                return new BaseResponseModel<RestoreTemplateResponse>
                {
                    Code = 200,
                    Message = "Template restored successfully",
                    Data = new RestoreTemplateResponse { Id = template.Id, Success = true, Message = "Restored" }
                };
            }
            catch (Exception ex) { return HandleException<RestoreTemplateResponse>(ex, "restoring template"); }
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

        private CustomShoeDesignTemplate GetEntityFromAddRequest(AddTemplateRequest request) => new()
        {
            Name = request.Name,
            Description = request.Description,
            Gender = request.Gender,
            Color = request.Color,
            Price = (float)request.BasePrice,
            TwoDImageUrl = request.PreviewImageUrl,
            ThreeDFileUrl = request.Model3DUrl,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsDeleted = false
        };
    }
}