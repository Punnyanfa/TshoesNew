using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Criteria;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.ManufacturerCriteriaService
{
    public class ManufacturerCriteriaService : IManufacturerCriteriaService
    {
        private readonly ICriteriaRepository _criteriaRepository;
        private readonly ILogger<ManufacturerCriteriaService> _logger;

        public ManufacturerCriteriaService(
            ICriteriaRepository criteriaRepository,
            ILogger<ManufacturerCriteriaService> logger)
        {
            _criteriaRepository = criteriaRepository ?? throw new ArgumentNullException(nameof(criteriaRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<AddCriteriaResponse>> AddCriteriaAsync(AddCriteriaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("Invalid criteria name: Name is empty");
                    return new BaseResponseModel<AddCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria name cannot be empty"
                    };
                }

                _logger.LogInformation("Adding new criteria: {Name}", request.Name);
                var criteria = new Criteria
                {
                    Name = request.Name,
                    Status = (CriteriaStatus)request.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedCriteria = await _criteriaRepository.AddAsync(criteria);
                _logger.LogInformation("Criteria added with ID: {Id}", addedCriteria.Id);

                return new BaseResponseModel<AddCriteriaResponse>
                {
                    Code = 201,
                    Message = "Criteria added successfully",
                    Data = new AddCriteriaResponse
                    {
                        Id = addedCriteria.Id,
                        Name = addedCriteria.Name,
                        CreatedAt = addedCriteria.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding criteria: {Name}", request.Name);
                return new BaseResponseModel<AddCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<GetCriteriaResponse>> GetCriteriaAsync(GetCriteriaRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid criteria ID: {Id}", request.Id);
                    return new BaseResponseModel<GetCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }

                _logger.LogInformation("Fetching criteria with ID: {Id}", request.Id);
                var criteria = await _criteriaRepository.FindAsync(request.Id);
                if (criteria == null || criteria.IsDeleted)
                {
                    _logger.LogWarning("Criteria not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<GetCriteriaResponse>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }

                return new BaseResponseModel<GetCriteriaResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new GetCriteriaResponse
                    {
                        Id = criteria.Id,
                        Name = criteria.Name,
                        Status = (int)criteria.Status,
                        CreatedAt = criteria.CreatedAt,
                        UpdatedAt = criteria.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching criteria with ID: {Id}", request.Id);
                return new BaseResponseModel<GetCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<UpdateCriteriaResponse>> UpdateCriteriaAsync(UpdateCriteriaRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid criteria ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("Invalid criteria name: Name is empty for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria name cannot be empty"
                    };
                }

                _logger.LogInformation("Updating criteria with ID: {Id}", request.Id);
                var criteria = await _criteriaRepository.FindAsync(request.Id);
                if (criteria == null || criteria.IsDeleted)
                {
                    _logger.LogWarning("Criteria not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }

                criteria.Name = request.Name;
                criteria.Status = (CriteriaStatus)request.Status;
                criteria.UpdatedAt = DateTime.UtcNow;

                await _criteriaRepository.UpdateAsync(criteria);
                _logger.LogInformation("Criteria updated with ID: {Id}", criteria.Id);

                return new BaseResponseModel<UpdateCriteriaResponse>
                {
                    Code = 200,
                    Message = "Criteria updated successfully",
                    Data = new UpdateCriteriaResponse
                    {
                        Id = criteria.Id,
                        Name = criteria.Name,
                        Status = (int)criteria.Status,
                        UpdatedAt = criteria.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating criteria with ID: {Id}", request.Id);
                return new BaseResponseModel<UpdateCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<bool>> DeleteCriteriaAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid criteria ID: {Id}", id);
                    return new BaseResponseModel<bool>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }

                _logger.LogInformation("Deleting criteria with ID: {Id}", id);
                var criteria = await _criteriaRepository.FindAsync(id);
                if (criteria == null || criteria.IsDeleted)
                {
                    _logger.LogWarning("Criteria not found for ID: {Id}", id);
                    return new BaseResponseModel<bool>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }

                criteria.IsDeleted = true;
                criteria.UpdatedAt = DateTime.UtcNow;
                await _criteriaRepository.UpdateAsync(criteria);
                _logger.LogInformation("Criteria marked as deleted with ID: {Id}", id);

                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Message = "Criteria deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting criteria with ID: {Id}", id);
                return new BaseResponseModel<bool>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponseModel<List<GetCriteriaResponse>>> GetAllActiveCriteriaAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all active criteria");
                var criteriaList = await _criteriaRepository.GetActiveCriteriaAsync();

                var responseData = criteriaList
                    .Where(c => !c.IsDeleted) // Ensure no deleted criteria
                    .Select(c => new GetCriteriaResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Status = (int)c.Status,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToList();

                return new BaseResponseModel<List<GetCriteriaResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active criteria");
                return new BaseResponseModel<List<GetCriteriaResponse>>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
    }
}