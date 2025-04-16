using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;

namespace FCSP.Services.ManufacturerService
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ManufacturerService> _logger;

        public ManufacturerService(IManufacturerRepository manufacturerRepository, IUserRepository userRepository, ILogger<ManufacturerService> logger)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetAllManufacturers()
        {
            try
            {
                _logger.LogInformation("Fetching all manufacturers with details");
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
                _logger.LogError(ex, "Error fetching all manufacturers");
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
                    _logger.LogWarning("Invalid Manufacturer ID: {Id}", request.Id);
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

                _logger.LogInformation("Fetching manufacturer with ID: {Id}", request.Id);
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
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
                _logger.LogError(ex, "Error fetching manufacturer with ID: {Id}", request.Id);
                return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<AddManufacturerResponse>> AddManufacturer(AddManufacturerRequest request)
        {
            try
            {
                _logger.LogInformation("Adding manufacturer for UserId: {UserId}", request.UserId);
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null || user.UserRole != UserRole.Manufacturer)
                {
                    _logger.LogWarning("User with ID {UserId} is not a Manufacturer", request.UserId);
                    return new BaseResponseModel<AddManufacturerResponse>
                    {
                        Code = 403,
                        Message = "Only users with Manufacturer role can be added as manufacturers"
                    };
                }

                var existingManufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(request.UserId);
                if (existingManufacturer != null)
                {
                    _logger.LogWarning("User with ID {UserId} already has a Manufacturer", request.UserId);
                    return new BaseResponseModel<AddManufacturerResponse>
                    {
                        Code = 409,
                        Message = "User already has a Manufacturer"
                    };
                }

                var manufacturer = new Manufacturer
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    CommissionRate = request.CommissionRate,
                    Status = (ManufacturerStatus)request.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedManufacturer = await _manufacturerRepository.AddAsync(manufacturer);
                _logger.LogInformation("Manufacturer added with ID: {Id}", addedManufacturer.Id);
                return new BaseResponseModel<AddManufacturerResponse>
                {
                    Code = 201,
                    Message = "Success",
                    Data = new AddManufacturerResponse
                    {
                        Id = addedManufacturer.Id,
                        Name = addedManufacturer.Name,
                        CreatedAt = addedManufacturer.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding manufacturer for UserId: {UserId}", request.UserId);
                return new BaseResponseModel<AddManufacturerResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<UpdateManufacturerResponse>> UpdateManufacturer(UpdateManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Manufacturer ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateManufacturerResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

                _logger.LogInformation("Updating manufacturer with ID: {Id}", request.Id);
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateManufacturerResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                manufacturer.Name = request.Name;
                manufacturer.CommissionRate = request.CommissionRate;
                manufacturer.Status = (ManufacturerStatus)request.Status;
                manufacturer.UpdatedAt = DateTime.UtcNow;

                await _manufacturerRepository.UpdateAsync(manufacturer);
                _logger.LogInformation("Manufacturer updated with ID: {Id}", manufacturer.Id);
                return new BaseResponseModel<UpdateManufacturerResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new UpdateManufacturerResponse
                    {
                        Id = manufacturer.Id,
                        Name = manufacturer.Name,
                        Status = (int)manufacturer.Status,
                        UpdatedAt = manufacturer.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating manufacturer with ID: {Id}", request.Id);
                return new BaseResponseModel<UpdateManufacturerResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<bool>> DeleteManufacturer(GetManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Manufacturer ID: {Id}", request.Id);
                    return new BaseResponseModel<bool> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

                _logger.LogInformation("Attempting to delete manufacturer with ID: {Id}", request.Id);
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<bool> { Code = 404, Message = "Manufacturer not found" };
                }

                manufacturer.Status = ManufacturerStatus.Inactive;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                await _manufacturerRepository.UpdateAsync(manufacturer);
                _logger.LogInformation("Manufacturer marked as Inactive with ID: {Id}", request.Id);
                return new BaseResponseModel<bool> { Code = 200, Message = "Manufacturer marked as inactive", Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting manufacturer with ID: {Id}", request.Id);
                return new BaseResponseModel<bool> { Code = 500, Message = ex.Message, Data = false };
            }
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetManufacturersByUserId(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    _logger.LogWarning("Invalid UserId: {UserId}", userId);
                    return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 400, Message = "User ID must be greater than 0" };
                }

                _logger.LogInformation("Fetching manufacturers for UserId: {UserId}", userId);
                var manufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(userId);
                if (manufacturer == null)
                {
                    _logger.LogWarning("No manufacturers found for UserId: {UserId}", userId);
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
                _logger.LogError(ex, "Error fetching manufacturers for UserId: {UserId}", userId);
                return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetActiveManufacturers()
        {
            try
            {
                _logger.LogInformation("Fetching active manufacturers");
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
                _logger.LogError(ex, "Error fetching active manufacturers");
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

            return new GetManufacturerDetailResponse(status: manufacturer.Status)
            {
                Id = manufacturer.Id,                
                UserName = user?.Name,
                Name = manufacturer.Name,
                CommissionRate = manufacturer.CommissionRate,              
                Services = manufacturer.Services?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
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