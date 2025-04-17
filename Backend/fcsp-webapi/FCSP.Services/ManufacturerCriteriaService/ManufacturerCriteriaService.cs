using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.ManufacturerCriteria;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.ManufacturerCriteriaService
{
    public class ManufacturerCriteriaService : IManufacturerCriteriaService
    {
        private readonly IManufacturerCriteriaRepository _manufacturerCriteriaRepository;
        private readonly ILogger<ManufacturerCriteriaService> _logger;

        public ManufacturerCriteriaService(
            IManufacturerCriteriaRepository manufacturerCriteriaRepository,
            ILogger<ManufacturerCriteriaService> logger)
        {
            _manufacturerCriteriaRepository = manufacturerCriteriaRepository;
            _logger = logger;
        }

        public async Task<BaseResponseModel<AddManufacturerCriteriaResponse>> AddManufacturerCriteriaAsync(AddManufacturerCriteriaRequest request)
        {
            try
            {
                if (request.ManufacturerId <= 0 || request.CriteriaId <= 0)
                {
                    _logger.LogWarning("Invalid ManufacturerId: {ManufacturerId} or CriteriaId: {CriteriaId}", request.ManufacturerId, request.CriteriaId);
                    return new BaseResponseModel<AddManufacturerCriteriaResponse>
                    {
                        Code = 400,
                        Message = "ManufacturerId and CriteriaId must be greater than 0"
                    };
                }

                if (!Enum.IsDefined(typeof(ManufacturerCriteriaStatus), request.Status))
                {
                    _logger.LogWarning("Invalid status: {Status}", request.Status);
                    return new BaseResponseModel<AddManufacturerCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Invalid status value  NotMet = 0,   Met = 1,    Pending = 2"
                    };
                }

                _logger.LogInformation("Adding new manufacturer-criteria: ManufacturerId={ManufacturerId}, CriteriaId={CriteriaId}", request.ManufacturerId, request.CriteriaId);
                var manufacturerCriteria = new ManufacturerCriteria
                {
                    ManufacturerId = request.ManufacturerId,
                    CriteriaId = request.CriteriaId,
                    Status = (ManufacturerCriteriaStatus)request.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedManufacturerCriteria = await _manufacturerCriteriaRepository.AddAsync(manufacturerCriteria);
                _logger.LogInformation("ManufacturerCriteria added with ID: {Id}", addedManufacturerCriteria.Id);

                return new BaseResponseModel<AddManufacturerCriteriaResponse>
                {
                    Code = 201,
                    Message = "ManufacturerCriteria added successfully",
                    Data = new AddManufacturerCriteriaResponse
                    {
                        Id = addedManufacturerCriteria.Id,
                        ManufacturerId = addedManufacturerCriteria.ManufacturerId,
                        CriteriaId = addedManufacturerCriteria.CriteriaId,
                        CreatedAt = addedManufacturerCriteria.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding manufacturer-criteria: ManufacturerId={ManufacturerId}, CriteriaId={CriteriaId}", request.ManufacturerId, request.CriteriaId);
                return new BaseResponseModel<AddManufacturerCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<GetManufacturerCriteriaResponse>> GetManufacturerCriteriaAsync(GetManufacturerCriteriaRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid ManufacturerCriteria ID: {Id}", request.Id);
                    return new BaseResponseModel<GetManufacturerCriteriaResponse>
                    {
                        Code = 400,
                        Message = "ManufacturerCriteria ID must be greater than 0"
                    };
                }

                _logger.LogInformation("Fetching ManufacturerCriteria with ID: {Id}", request.Id);
                var manufacturerCriteria = await _manufacturerCriteriaRepository.GetAll()
                    .Include(mc => mc.Manufacturer)
                    .Include(mc => mc.Criteria)
                    .FirstOrDefaultAsync(mc => mc.Id == request.Id && !mc.IsDeleted);

                if (manufacturerCriteria == null)
                {
                    _logger.LogWarning("ManufacturerCriteria not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<GetManufacturerCriteriaResponse>
                    {
                        Code = 404,
                        Message = "ManufacturerCriteria not found"
                    };
                }

                return new BaseResponseModel<GetManufacturerCriteriaResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = MapToDetailResponse(manufacturerCriteria)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ManufacturerCriteria with ID: {Id}", request.Id);
                return new BaseResponseModel<GetManufacturerCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<UpdateManufacturerCriteriaResponse>> UpdateManufacturerCriteriaAsync(UpdateManufacturerCriteriaRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid ManufacturerCriteria ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateManufacturerCriteriaResponse>
                    {
                        Code = 400,
                        Message = "ManufacturerCriteria ID must be greater than 0"
                    };
                }

                if (!Enum.IsDefined(typeof(ManufacturerCriteriaStatus), request.Status))
                {
                    _logger.LogWarning("Invalid status: {Status}", request.Status);
                    return new BaseResponseModel<UpdateManufacturerCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Invalid status value  NotMet = 0,   Met = 1,    Pending = 2"
                    };
                }

                _logger.LogInformation("Updating ManufacturerCriteria with ID: {Id}", request.Id);
                var manufacturerCriteria = await _manufacturerCriteriaRepository.FindAsync(request.Id);
                if (manufacturerCriteria == null || manufacturerCriteria.IsDeleted)
                {
                    _logger.LogWarning("ManufacturerCriteria not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateManufacturerCriteriaResponse>
                    {
                        Code = 404,
                        Message = "ManufacturerCriteria not found"
                    };
                }

                manufacturerCriteria.Status = (ManufacturerCriteriaStatus)request.Status;
                manufacturerCriteria.UpdatedAt = DateTime.UtcNow;

                await _manufacturerCriteriaRepository.UpdateAsync(manufacturerCriteria);
                _logger.LogInformation("ManufacturerCriteria updated with ID: {Id}", manufacturerCriteria.Id);

                return new BaseResponseModel<UpdateManufacturerCriteriaResponse>
                {
                    Code = 200,
                    Message = "ManufacturerCriteria updated successfully",
                    Data = new UpdateManufacturerCriteriaResponse
                    {
                        Id = manufacturerCriteria.Id,
                        Status = manufacturerCriteria.Status,
                        UpdatedAt = manufacturerCriteria.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ManufacturerCriteria with ID: {Id}", request.Id);
                return new BaseResponseModel<UpdateManufacturerCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<bool>> DeleteManufacturerCriteriaAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid ManufacturerCriteria ID: {Id}", id);
                    return new BaseResponseModel<bool>
                    {
                        Code = 400,
                        Message = "ManufacturerCriteria ID must be greater than 0"
                    };
                }

                _logger.LogInformation("Deleting ManufacturerCriteria with ID: {Id}", id);
                var manufacturerCriteria = await _manufacturerCriteriaRepository.FindAsync(id);
                if (manufacturerCriteria == null || manufacturerCriteria.IsDeleted)
                {
                    _logger.LogWarning("ManufacturerCriteria not found for ID: {Id}", id);
                    return new BaseResponseModel<bool>
                    {
                        Code = 404,
                        Message = "ManufacturerCriteria not found"
                    };
                }

                await _manufacturerCriteriaRepository.DeleteAsync(id);
                _logger.LogInformation("ManufacturerCriteria deleted with ID: {Id}", id);

                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Message = "ManufacturerCriteria deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ManufacturerCriteria with ID: {Id}", id);
                return new BaseResponseModel<bool>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponseModel<List<GetManufacturerCriteriaResponse>>> GetAllManufacturerCriteriaAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all ManufacturerCriteria");
                var manufacturerCriteriaList = await _manufacturerCriteriaRepository.GetAll()
                    .Include(mc => mc.Manufacturer)
                    .Include(mc => mc.Criteria)
                    .Where(mc => !mc.IsDeleted)
                    .ToListAsync();

                var responseData = manufacturerCriteriaList.Select(MapToDetailResponse).ToList();

                return new BaseResponseModel<List<GetManufacturerCriteriaResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all ManufacturerCriteria");
                return new BaseResponseModel<List<GetManufacturerCriteriaResponse>>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        private GetManufacturerCriteriaResponse MapToDetailResponse(ManufacturerCriteria manufacturerCriteria)
        { 
            return new GetManufacturerCriteriaResponse
            {
                Id = manufacturerCriteria.Id,
                ManufacturerId = manufacturerCriteria.ManufacturerId,
                CriteriaId = manufacturerCriteria.CriteriaId,
                Status = manufacturerCriteria.Status,
                ManufacturerName = manufacturerCriteria.Manufacturer?.Name ?? "Unknown",
                CriteriaName = manufacturerCriteria.Criteria?.Name ?? "Unknown",
                CreatedAt = manufacturerCriteria.CreatedAt,
                UpdatedAt = manufacturerCriteria.UpdatedAt
            };
        }
    }
}