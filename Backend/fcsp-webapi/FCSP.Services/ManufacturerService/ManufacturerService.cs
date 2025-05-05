using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCSP.Services.ManufacturerService
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ManufacturerService> _logger;

        public ManufacturerService(IManufacturerRepository manufacturerRepository, IUserRepository userRepository)
        {
            _manufacturerRepository = manufacturerRepository;
            _userRepository = userRepository;
            
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetAllManufacturers()
        {
            try
            {
                
                var manufacturers = await _manufacturerRepository.GetAllWithDetailsAsync();

                var detailedManufacturers = new List<GetManufacturerDetailResponse>();
                foreach (var manufacturer in manufacturers)
                {
                    var response = await MapToDetailResponse(manufacturer);
                    detailedManufacturers.Add(response);
                }
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = detailedManufacturers
                };
            }
            catch (Exception ex)
            {
               
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponseModel<GetManufacturerDetailResponse>> GetManufacturerById(GetManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                   
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

               
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                return new BaseResponseModel<GetManufacturerDetailResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = await MapToDetailResponse(manufacturer)
                };
            }
            catch (Exception ex)
            {
                
                return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 500, Message = ex.Message };
            }
        }
        public async Task<BaseResponseModel<AddManufacturerResponse>> AddManufacturer(AddManufacturerRequest request)
        {
            try
            {              
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null || user.UserRole != UserRole.Manufacturer)
                {                   
                    return new BaseResponseModel<AddManufacturerResponse>
                    {
                        Code = 403,
                        Message = "Only users with Manufacturer role can be added as manufacturers"
                    };
                }

                var existingManufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(request.UserId);
                if (existingManufacturer != null)
                {
                    return new BaseResponseModel<AddManufacturerResponse>
                    {
                        Code = 409,
                        Message = "User already has a Manufacturer"
                    };
                }

                var manufacturer = new Manufacturer
                {
                    UserId = request.UserId,
                    Description = request.Description,
                    CommissionRate = request.CommissionRate,
                    Status = (ManufacturerStatus)request.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedManufacturer = await _manufacturerRepository.AddAsync(manufacturer);
                return new BaseResponseModel<AddManufacturerResponse>
                {
                    Code = 201,
                    Message = "Success",
                    Data = new AddManufacturerResponse
                    {
                        Id = addedManufacturer.Id,
                        Description = addedManufacturer.Description,
                        CreatedAt = addedManufacturer.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddManufacturerResponse> { Code = 500, Message = ex.Message };
            }
        }
        public async Task<BaseResponseModel<UpdateManufacturerResponse>> UpdateManufacturer(UpdateManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return new BaseResponseModel<UpdateManufacturerResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    return new BaseResponseModel<UpdateManufacturerResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                manufacturer.Description = request.Description;
                manufacturer.CommissionRate = request.CommissionRate;
                manufacturer.Status = (ManufacturerStatus)request.Status;
                manufacturer.UpdatedAt = DateTime.UtcNow;

                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<UpdateManufacturerResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new UpdateManufacturerResponse
                    {
                        Id = manufacturer.Id,
                        Description = manufacturer.Description,
                        Status = manufacturer.Status.ToString(),
                        UpdatedAt = manufacturer.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateManufacturerResponse> { Code = 500, Message = ex.Message };
            }
        }
        public async Task<BaseResponseModel<bool>> DeleteManufacturer(GetManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return new BaseResponseModel<bool> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    return new BaseResponseModel<bool> { Code = 404, Message = "Manufacturer not found" };
                }

                manufacturer.Status = ManufacturerStatus.Inactive;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<bool> { Code = 200, Message = "Manufacturer marked as inactive", Data = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool> { Code = 500, Message = ex.Message, Data = false };
            }
        }
        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetManufacturersByUserId(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 400, Message = "User ID must be greater than 0" };
                }
                var manufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(userId);
                if (manufacturer == null)
                {
                    return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 404, Message = "No manufacturers found for this user" };
                }

                var responseData = new List<GetManufacturerDetailResponse> { await MapToDetailResponse(manufacturer) };
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 500, Message = ex.Message };
            }
        }
        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetActiveManufacturers()
        {
            try
            {
                var manufacturers = await _manufacturerRepository.GetManufacturersByStatusAsync((int)ManufacturerStatus.Active);

                var detailedManufacturers = new List<GetManufacturerDetailResponse>();
                foreach (var manufacturer in manufacturers)
                {
                    var response = await MapToDetailResponse(manufacturer);
                    detailedManufacturers.Add(response);
                }

                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = detailedManufacturers
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        private async Task<GetManufacturerDetailResponse> MapToDetailResponse(Manufacturer manufacturer)
        {
            var user = await _userRepository.GetUserNameByUserIdAsync(manufacturer.UserId);

            return new GetManufacturerDetailResponse
            {
                Id = manufacturer.Id,
                UserName = user?.Name,
                Description = manufacturer.Description,
                Status = manufacturer.Status.ToString(),
                CommissionRate = manufacturer.CommissionRate,
                Services = manufacturer.Services?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CurrentAmount = s.SetServiceAmounts?
                        .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow))?.Amount
                }).ToList() ?? new List<ServiceDto>(),
                Criterias = manufacturer.ManufacturerCriterias?.Select(mc => new CriteriaDto
                {
                    Id = mc.CriteriaId,
                    Name = mc.Criteria.Name
                }).ToList() ?? new List<CriteriaDto>(),
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt
            };
        }
    }
}