using FCSP.DTOs;
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
            catch (Exception ex)
            {
                return new BaseResponseModel<List<CustomShoeDesignTemplate>>
                {
                    Code = 500,
                    Message = $"Error retrieving templates: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<GetTemplateByIdResponse>> GetTemplateById(GetTemplateByIdRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                {
                    return new BaseResponseModel<GetTemplateByIdResponse>
                    {
                        Code = 404,
                        Message = "Template not found"
                    };
                }

                return new BaseResponseModel<GetTemplateByIdResponse>
                {
                    Code = 200,
                    Message = "Template retrieved successfully",
                    Data = new GetTemplateByIdResponse
                    {
                        Id = template.Id,
                        Name = template.Name,
                        Description = template.Description ?? string.Empty,
                        PreviewImageUrl = template.TwoDImageUrl ?? string.Empty,
                        Model3DUrl = template.ThreeDFileUrl ?? string.Empty,
                        BasePrice = (decimal)template.Price,
                        IsAvailable = true,
                        CreatedAt = template.CreatedAt,
                        UpdatedAt = template.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetTemplateByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving template: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddTemplateResponse>> AddTemplate(AddTemplateRequest request)
        {
            try
            {
                CustomShoeDesignTemplate template = GetEntityFromAddRequest(request);
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
            catch (Exception ex)
            {
                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 500,
                    Message = $"Error creating template: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddTemplateResponse>> UpdateTemplate(UpdateTemplateRequest request)
        {
            try
            {
                var template = _templateRepository.Find(request.Id);
                if (template == null)
                {
                    return new BaseResponseModel<AddTemplateResponse>
                    {
                        Code = 404,
                        Message = "Template not found"
                    };
                }

                template.Name = request.Name;
                template.Description = request.Description;
                template.Price = (float)request.BasePrice;
                template.TwoDImageUrl = request.PreviewImageUrl;
                template.ThreeDFileUrl = request.Model3DUrl;
                template.UpdatedAt = DateTime.Now;

                await _templateRepository.UpdateAsync(template);
                
                return new BaseResponseModel<AddTemplateResponse>
                {
                    Code = 200,
                    Message = "Template updated successfully",
                    Data = new AddTemplateResponse
                    {
                        Id = template.Id,
                        Name = template.Name,
                        PreviewImageUrl = template.TwoDImageUrl ?? string.Empty
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddTemplateResponse>
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
                {
                    return new BaseResponseModel<DeleteTemplateResponse>
                    {
                        Code = 404,
                        Message = "Template not found"
                    };
                }

                await _templateRepository.DeleteAsync(template.Id);
                
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
                    Message = $"Error deleting template: {ex.Message}",
                    Data = new DeleteTemplateResponse { Success = false }
                };
            }
        }

        private CustomShoeDesignTemplate GetEntityFromAddRequest(AddTemplateRequest request)
        {
            return new CustomShoeDesignTemplate
            {
                Name = request.Name,
                Description = request.Description,
                Price = (float)request.BasePrice,
                TwoDImageUrl = request.PreviewImageUrl,
                ThreeDFileUrl = request.Model3DUrl,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
