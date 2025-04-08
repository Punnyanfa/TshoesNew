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

        public ManufacturerService(IManufacturerRepository manufacturerRepository, IUserRepository userRepository, ILogger<ManufacturerService> logger)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<List<Manufacturer>>> GetAllManufacturers()
        {
            try
            {
                _logger.LogInformation("Fetching all manufacturers");
                var manufacturers = await _manufacturerRepository.GetAllAsync();
                return new BaseResponseModel<List<Manufacturer>> { Code = 200, Message = "Success", Data = manufacturers.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all manufacturers");
                return new BaseResponseModel<List<Manufacturer>> { Code = 500, Message = ex.Message };
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
                var manufacturer = await _manufacturerRepository.FindAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                return new BaseResponseModel<GetManufacturerDetailResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = MapToDetailResponse(manufacturer)
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

                var manufacturer = new Manufacturer
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    CommissionRate = request.CommissionRate,
                    Status = (ManufacturerStatus)request.Status
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
                        CreatedAt = DateTime.UtcNow
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
                var manufacturer = await _manufacturerRepository.FindAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateManufacturerResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                manufacturer.Name = request.Name;
                manufacturer.CommissionRate = request.CommissionRate;
                manufacturer.Status = (ManufacturerStatus)request.Status;

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
                        UpdatedAt = DateTime.UtcNow
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
                var manufacturer = await _manufacturerRepository.FindAsync(request.Id);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<bool> { Code = 404, Message = "Manufacturer not found" };
                }

                // Thay vì xóa cứng, cập nhật Status thành Inactive
                manufacturer.Status = ManufacturerStatus.Inactive;
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
                var manufacturers = await _manufacturerRepository.GetAllAsync();
                var userManufacturers = manufacturers.Where(m => m.UserId == userId).ToList();

                if (!userManufacturers.Any())
                {
                    _logger.LogWarning("No manufacturers found for UserId: {UserId}", userId);
                    return new BaseResponseModel<List<GetManufacturerDetailResponse>> { Code = 404, Message = "No manufacturers found for this user" };
                }

                var responseData = userManufacturers.Select(MapToDetailResponse).ToList();
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

        public async Task<BaseResponseModel<List<Manufacturer>>> GetActiveManufacturers()
        {
            try
            {
                _logger.LogInformation("Fetching active manufacturers");
                var manufacturers = await _manufacturerRepository.GetManufacturersByStatusAsync((int)ManufacturerStatus.Active);
                return new BaseResponseModel<List<Manufacturer>> { Code = 200, Message = "Success", Data = manufacturers.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active manufacturers");
                return new BaseResponseModel<List<Manufacturer>> { Code = 500, Message = ex.Message };
            }
        }

        private GetManufacturerDetailResponse MapToDetailResponse(Manufacturer manufacturer)
        {
            return new GetManufacturerDetailResponse
            {
                Id = manufacturer.Id,
                UserId = manufacturer.UserId,
                Name = manufacturer.Name,
                CommissionRate = manufacturer.CommissionRate,
                Status = (int)manufacturer.Status,
                CreatedAt = DateTime.UtcNow, // Giả định
                UpdatedAt = DateTime.UtcNow, // Giả định
                Services = manufacturer.Services.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    CurrentAmount = s.SetServiceAmounts
                        .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow))?.Amount
                }).ToList(),
                Criterias = manufacturer.ManufacturerCriterias.Select(mc => new CriteriaDto
                {
                    Id = mc.CriteriaId,
                    Name = mc.Criteria.Name
                }).ToList()
            };
        }
    }
}