using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCSP.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository; // Giữ lại IUserRepository
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(IServiceRepository serviceRepository, IManufacturerRepository manufacturerRepository, IUserRepository userRepository, ILogger<ServiceService> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<List<Service>>> GetAllServices()
        {
            try
            {
                _logger.LogInformation("Fetching all active services");
                var services = await _serviceRepository.GetActiveServicesAsync();
                return new BaseResponseModel<List<Service>> { Code = 200, Message = "Success", Data = services.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching services");
                return new BaseResponseModel<List<Service>> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<GetServiceByIdResponse>> GetServiceById(GetServiceByIdRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<GetServiceByIdResponse> { Code = 400, Message = "Service ID must be greater than 0" };
                }

                _logger.LogInformation("Fetching service with ID: {Id}", request.Id);
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<GetServiceByIdResponse> { Code = 404, Message = "Service not found" };
                }

                var currentAmount = service.SetServiceAmounts
                    .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));

                return new BaseResponseModel<GetServiceByIdResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new GetServiceByIdResponse
                    {
                        Id = service.Id,
                        Name = service.ServiceName,
                        Price = currentAmount?.Amount ?? 0,
                        IsDeleted = service.IsDeleted
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching service with ID: {Id}", request.Id);
                return new BaseResponseModel<GetServiceByIdResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<AddServiceResponse>> AddService(AddServiceRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("Service name is required");
                    return new BaseResponseModel<AddServiceResponse> { Code = 400, Message = "Service name is required" };
                }
                if (request.Name.Length < 2 || request.Name.Length > 100)
                {
                    _logger.LogWarning("Invalid service name length: {Length}", request.Name.Length);
                    return new BaseResponseModel<AddServiceResponse> { Code = 400, Message = "Service name must be between 2 and 100 characters" };
                }
                if (request.Price < 0)
                {
                    _logger.LogWarning("Negative price provided: {Price}", request.Price);
                    return new BaseResponseModel<AddServiceResponse> { Code = 400, Message = "Price cannot be negative" };
                }
                if (request.ManufacturerId <= 0)
                {
                    _logger.LogWarning("Invalid Manufacturer ID: {Id}", request.ManufacturerId);
                    return new BaseResponseModel<AddServiceResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

                // Kiểm tra Manufacturer
                _logger.LogInformation("Checking Manufacturer with ID: {ManufacturerId}", request.ManufacturerId);
                var manufacturer = await _manufacturerRepository.FindAsync(request.ManufacturerId);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {ManufacturerId}", request.ManufacturerId);
                    return new BaseResponseModel<AddServiceResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                // Kiểm tra UserRole thông qua UserId trong Manufacturer
                var user = await _userRepository.GetByIdAsync(manufacturer.UserId);
                if (user == null || user.UserRole != UserRole.Manufacturer)
                {
                    _logger.LogWarning("User with ID {UserId} is not a Manufacturer", manufacturer.UserId);
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Only users with Manufacturer role can add services"
                    };
                }

                // Kiểm tra Status của Manufacturer
                if (manufacturer.Status != ManufacturerStatus.Active)
                {
                    _logger.LogWarning("Manufacturer with ID {ManufacturerId} is not Active (Status: {Status})", request.ManufacturerId, manufacturer.Status);
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Services can only be added by Manufacturers with Active status"
                    };
                }

                _logger.LogInformation("Adding service for ManufacturerId: {ManufacturerId}", request.ManufacturerId);
                var service = new Service
                {
                    ServiceName = request.Name,
                    IsDeleted = false,
                    ManufacturerId = request.ManufacturerId
                };

                service.SetServiceAmounts.Add(new SetServiceAmount
                {
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    Amount = request.Price,
                    Status = ServiceAmountStatus.Active
                });

                var addedService = await _serviceRepository.AddAsync(service);
                service.SetServiceAmounts.First().ServiceId = addedService.Id;
                await _serviceRepository.UpdateAsync(service);

                _logger.LogInformation("Service added successfully with ID: {ServiceId}", addedService.Id);
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 201,
                    Message = "Success",
                    Data = new AddServiceResponse { ServiceId = addedService.Id }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service for ManufacturerId: {ManufacturerId}", request.ManufacturerId);
                return new BaseResponseModel<AddServiceResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateServiceResponse> { Code = 400, Message = "Service ID must be greater than 0" };
                }
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("Service name is required");
                    return new BaseResponseModel<UpdateServiceResponse> { Code = 400, Message = "Service name is required" };
                }
                if (request.Name.Length < 2 || request.Name.Length > 100)
                {
                    _logger.LogWarning("Invalid service name length: {Length}", request.Name.Length);
                    return new BaseResponseModel<UpdateServiceResponse> { Code = 400, Message = "Service name must be between 2 and 100 characters" };
                }
                if (request.Price < 0)
                {
                    _logger.LogWarning("Negative price provided: {Price}", request.Price);
                    return new BaseResponseModel<UpdateServiceResponse> { Code = 400, Message = "Price cannot be negative" };
                }

                _logger.LogInformation("Updating service with ID: {Id}", request.Id);
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateServiceResponse> { Code = 404, Message = "Service not found" };
                }

                service.ServiceName = request.Name;
                var currentAmount = service.SetServiceAmounts
                    .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
                if (currentAmount != null && currentAmount.Amount != request.Price)
                {
                    currentAmount.EndDate = DateTime.UtcNow;
                    currentAmount.Status = ServiceAmountStatus.Expired;
                    service.SetServiceAmounts.Add(new SetServiceAmount
                    {
                        ServiceId = service.Id,
                        StartDate = DateTime.UtcNow,
                        EndDate = null,
                        Amount = request.Price,
                        Status = ServiceAmountStatus.Active
                    });
                }
                else if (currentAmount == null)
                {
                    service.SetServiceAmounts.Add(new SetServiceAmount
                    {
                        ServiceId = service.Id,
                        StartDate = DateTime.UtcNow,
                        EndDate = null,
                        Amount = request.Price,
                        Status = ServiceAmountStatus.Active
                    });
                }

                await _serviceRepository.UpdateAsync(service);
                _logger.LogInformation("Service updated successfully with ID: {Id}", service.Id);
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new UpdateServiceResponse { ServiceId = service.Id }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID: {Id}", request.Id);
                return new BaseResponseModel<UpdateServiceResponse> { Code = 500, Message = ex.Message };
            }
        }

        public async Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteServiceResponse> { Code = 400, Message = "Service ID must be greater than 0" };
                }

                _logger.LogInformation("Deleting service with ID: {Id}", request.Id);
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteServiceResponse> { Code = 404, Message = "Service not found" };
                }

                service.IsDeleted = true;
                await _serviceRepository.UpdateAsync(service);
                _logger.LogInformation("Service deleted successfully with ID: {Id}", request.Id);
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new DeleteServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service with ID: {Id}", request.Id);
                return new BaseResponseModel<DeleteServiceResponse> { Code = 500, Message = ex.Message };
            }
        }
    }
}