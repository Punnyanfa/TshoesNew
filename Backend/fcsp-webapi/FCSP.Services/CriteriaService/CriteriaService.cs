using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Criteria;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.CriteriaService
{
    public class CriteriaService : ICriteriaService
    {
        private readonly ICriteriaRepository _criteriaRepository;
  

        public CriteriaService(ICriteriaRepository criteriaRepository, ILogger<CriteriaService> logger)
        {
            _criteriaRepository = criteriaRepository;
  
        }
        public async Task<BaseResponseModel<AddCriteriaResponse>> AddAsync(AddCriteriaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {                  
                    return new BaseResponseModel<AddCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria name cannot be empty"
                    };
                }              
                var existingCriteria = await _criteriaRepository.GetAll()
                    .Where(c => !c.IsDeleted && c.Name.ToLower() == request.Name.ToLower())
                    .FirstOrDefaultAsync();
                if (existingCriteria != null)
                {                 
                    return new BaseResponseModel<AddCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria with the same name already exists"
                    };
                }            
                var criteria = new Criteria
                {
                    Name = request.Name,
                    Status = CriteriaStatus.Active, 
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedCriteria = await _criteriaRepository.AddAsync(criteria);
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
                return new BaseResponseModel<AddCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponseModel<UpdateCriteriaResponse>> UpdateAsync(UpdateCriteriaRequest request)
        {
            try
            {
              
                if (request.Id <= 0)
                {                 
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }
               
                if (string.IsNullOrWhiteSpace(request.Name))
                {                 
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria name cannot be empty"
                    };
                }                                                 
                var criteria = await _criteriaRepository.FindAsync(request.Id);
                if (criteria == null || criteria.IsDeleted)
                {                   
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }
                
                var existingCriteria = await _criteriaRepository.GetAll()
                    .Where(c => !c.IsDeleted && c.Name.ToLower() == request.Name.ToLower() && c.Id != request.Id)
                    .FirstOrDefaultAsync();
                if (existingCriteria != null)
                {                   
                    return new BaseResponseModel<UpdateCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria with the same name already exists"
                    };
                }

                criteria.Name = request.Name;              
                criteria.UpdatedAt = DateTime.UtcNow;

                await _criteriaRepository.UpdateAsync(criteria);
              

                return new BaseResponseModel<UpdateCriteriaResponse>
                {
                    Code = 200,
                    Message = "Criteria updated successfully",
                    Data = new UpdateCriteriaResponse(status: criteria.Status)
                    {
                        Id = criteria.Id,
                        Name = criteria.Name,                      
                        UpdatedAt = criteria.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {             
                return new BaseResponseModel<UpdateCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponseModel<UpdateCriteriaStatusResponse>> UpdateStatusAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return new BaseResponseModel<UpdateCriteriaStatusResponse>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }
                var criteria = await _criteriaRepository.FindAsync(id);
                if (criteria == null)
                {
                    return new BaseResponseModel<UpdateCriteriaStatusResponse>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }
                criteria.IsDeleted = false;
                criteria.Status = CriteriaStatus.Active;
                criteria.UpdatedAt = DateTime.UtcNow;

                await _criteriaRepository.UpdateAsync(criteria);

                return new BaseResponseModel<UpdateCriteriaStatusResponse>
                {
                    Code = 200,
                    Message = "Criteria status updated successfully",
                    Data = new UpdateCriteriaStatusResponse
                    {
                        Id = criteria.Id,
                        Status = criteria.Status,
                        UpdatedAt = criteria.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateCriteriaStatusResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponseModel<bool>> DeleteAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return new BaseResponseModel<bool>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }
                var criteria = await _criteriaRepository.FindAsync(id);
                if (criteria == null || criteria.IsDeleted)
                {
                    return new BaseResponseModel<bool>
                    {
                        Code = 404,
                        Message = "Criteria not found"
                    };
                }
                criteria.Status = CriteriaStatus.Inactive;
                await _criteriaRepository.DeleteAsync(id);
                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Message = "Criteria deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = false
                };
            }
        }
        public async Task<BaseResponseModel<IList<GetCriteriaResponse>>> GetAllAsync()
        {
            try
            {
                var criteriaList = await _criteriaRepository.GetActiveCriteriaAsync();
                var responseData = criteriaList.Select(MapToDetailResponse).ToList();
                return new BaseResponseModel<IList<GetCriteriaResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IList<GetCriteriaResponse>>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponseModel<GetCriteriaResponse>> GetByIdAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return new BaseResponseModel<GetCriteriaResponse>
                    {
                        Code = 400,
                        Message = "Criteria ID must be greater than 0"
                    };
                }
                var criteria = await _criteriaRepository.FindAsync(id);
                if (criteria == null || criteria.IsDeleted)
                {
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
                    Data = MapToDetailResponse(criteria)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetCriteriaResponse>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        private GetCriteriaResponse MapToDetailResponse(Criteria criteria)
        {
            return new GetCriteriaResponse(status: criteria.Status)
            {
                Id = criteria.Id,
                Name = criteria.Name,
                CreatedAt = criteria.CreatedAt,
                UpdatedAt = criteria.UpdatedAt
            };
        }
    }
}