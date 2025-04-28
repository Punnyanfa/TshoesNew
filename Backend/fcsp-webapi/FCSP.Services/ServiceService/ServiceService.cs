using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FCSP.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(
            IServiceRepository serviceRepository,
            IManufacturerRepository manufacturerRepository,
            IUserRepository userRepository,
            ILogger<ServiceService> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<List<ServiceResponseDto>>> GetAllServices()
        {
            try
            {
                _logger.LogInformation("Fetching all active services");
                var services = await _serviceRepository.GetActiveServicesAsync();
                if (services == null || !services.Any())
                {
                    _logger.LogWarning("No active services found");
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 404,
                        Message = "No active services found",
                        Data = null
                    };
                }

                var result = services.Select(s =>
                {
                    var currentAmount = s.SetServiceAmounts
                        .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
                    return new ServiceResponseDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Component = s.Component,
                        Price = currentAmount?.Amount ?? 0,
                        ManufacturerId = s.ManufacturerId,
                        IsDeleted = s.IsDeleted
                    };
                }).ToList();

                return new BaseResponseModel<List<ServiceResponseDto>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching services");
                return new BaseResponseModel<List<ServiceResponseDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<BaseResponseModel<List<ServiceResponseDto>>> GetServicesByManufacturerId(long manufacturerId)
        {
            try
            {
                if (manufacturerId <= 0)
                {
                    _logger.LogWarning("Invalid Manufacturer ID: {ManufacturerId}", manufacturerId);
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 400,
                        Message = "Manufacturer ID must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Fetching services for ManufacturerId: {ManufacturerId}", manufacturerId);
                var services = await _serviceRepository.GetByManufacturerIdAsync(manufacturerId);
                if (services == null || !services.Any())
                {
                    _logger.LogWarning("No services found for ManufacturerId: {ManufacturerId}", manufacturerId);
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 404,
                        Message = "No services found for this manufacturer",
                        Data = null
                    };
                }

                var result = services.Select(s =>
                {
                    var currentAmount = s.SetServiceAmounts
                        .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
                    return new ServiceResponseDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Component = s.Component,
                        Price = currentAmount?.Amount ?? 0,
                        ManufacturerId = s.ManufacturerId,
                        IsDeleted = s.IsDeleted
                    };
                }).ToList();

                return new BaseResponseModel<List<ServiceResponseDto>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching services for ManufacturerId: {ManufacturerId}", manufacturerId);
                return new BaseResponseModel<List<ServiceResponseDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<ServiceResponseDto>> GetServiceById(GetServiceByIdRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<ServiceResponseDto>
                    {
                        Code = 400,
                        Message = "Service ID must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Fetching service with ID: {Id}", request.Id);
                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<ServiceResponseDto>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                var currentAmount = service.SetServiceAmounts
                    .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));

                return new BaseResponseModel<ServiceResponseDto>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new ServiceResponseDto
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Description = service.Description,
                        Component = service.Component,
                        Price = currentAmount?.Amount ?? 0,
                        ManufacturerId = service.ManufacturerId,
                        IsDeleted = service.IsDeleted
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching service with ID: {Id}", request.Id);
                return new BaseResponseModel<ServiceResponseDto>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddServiceResponse>> AddService(AddServiceRequest request)
        {
            try
            {
                var validationResult = ValidateServiceInput<AddServiceResponse>(
                    request.Name, request.Description, request.Component, request.Price, request.ManufacturerId);
                if (validationResult != null) return validationResult;

                _logger.LogInformation("Checking Manufacturer with ID: {ManufacturerId}", request.ManufacturerId);
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.ManufacturerId);
                if (manufacturer == null)
                {
                    _logger.LogWarning("Manufacturer not found for ID: {ManufacturerId}", request.ManufacturerId);
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 404,
                        Message = "Manufacturer not found",
                        Data = null
                    };
                }

                var user = await _userRepository.GetByIdAsync(manufacturer.UserId);
                if (user == null || user.UserRole != UserRole.Manufacturer)
                {
                    _logger.LogWarning("User with ID {UserId} is not a Manufacturer", manufacturer.UserId);
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Only users with Manufacturer role can add services",
                        Data = null
                    };
                }

                if (manufacturer.Status != ManufacturerStatus.Active)
                {
                    _logger.LogWarning("Manufacturer with ID {ManufacturerId} is not Active (Status: {Status})", request.ManufacturerId, manufacturer.Status);
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Services can only be added by Manufacturers with Active status",
                        Data = null
                    };
                }

                _logger.LogInformation("Adding service for ManufacturerId: {ManufacturerId}", request.ManufacturerId);
                var service = new Service
                {
                    Name = request.Name,
                    Description = request.Description,
                    Component = request.Component,
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
                    Data = new AddServiceResponse
                    {
                        Id = addedService.Id,
                        Name = addedService.Name,
                        Description = addedService.Description,
                        Component = addedService.Component,
                        Price = request.Price,
                        ManufacturerId = addedService.ManufacturerId,
                        IsDeleted = addedService.IsDeleted
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service for ManufacturerId: {ManufacturerId}", request.ManufacturerId);
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateServiceResponse>
                    {
                        Code = 400,
                        Message = "Service ID must be greater than 0",
                        Data = null
                    };
                }

                var validationResult = ValidateServiceInput<UpdateServiceResponse>(
                    request.Name, request.Description, request.Component, request.Price);
                if (validationResult != null) return validationResult;

                _logger.LogInformation("Updating service with ID: {Id}", request.Id);
                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                service.Name = request.Name;
                service.Description = request.Description;
                service.Component = request.Component;
                
                var currentAmount = service.SetServiceAmounts
                    .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
                if (currentAmount?.Amount != request.Price)
                {
                    if (currentAmount != null)
                    {
                        currentAmount.EndDate = DateTime.UtcNow;
                        currentAmount.Status = ServiceAmountStatus.Expired;
                    }
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
                    Data = new UpdateServiceResponse
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Description = service.Description,
                        Component = service.Component,
                        Price = request.Price,
                        ManufacturerId = service.ManufacturerId,
                        IsDeleted = service.IsDeleted
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID: {Id}", request.Id);
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid Service ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteServiceResponse>
                    {
                        Code = 400,
                        Message = "Service ID must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Deleting service with ID: {Id}", request.Id);
                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
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
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<float?> GetServicePriceAsync(long serviceId)
        {
            try
            {
                _logger.LogInformation("Fetching price for service with ID: {ServiceId}", serviceId);
                var service = await _serviceRepository.GetServiceWithDetailsAsync(serviceId);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {ServiceId}", serviceId);
                    return null;
                }

                if (!service.SetServiceAmounts.Any())
                {
                    _logger.LogWarning("No SetServiceAmounts found for Service with ID: {ServiceId}", serviceId);
                    return null;
                }

                var currentAmount = service.SetServiceAmounts
                    .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow));

                if (currentAmount == null)
                {
                    _logger.LogWarning("No active SetServiceAmount found for Service with ID: {ServiceId}. Existing statuses: {Statuses}, EndDates: {EndDates}",
                        serviceId,
                        string.Join(", ", service.SetServiceAmounts.Select(a => a.Status)),
                        string.Join(", ", service.SetServiceAmounts.Select(a => a.EndDate?.ToString() ?? "null")));
                    return null;
                }

                _logger.LogInformation("Found active price {Price} for Service with ID: {ServiceId}", currentAmount.Amount, serviceId);
                return currentAmount.Amount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching price for service with ID: {ServiceId}", serviceId);
                return null;
            }
        }

        private BaseResponseModel<T>? ValidateServiceInput<T>(string name, float price, long manufacturerId = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Service name is required");
                return new BaseResponseModel<T> { Code = 400, Message = "Service name is required", Data = default };
            }
            if (name.Length < 2 || name.Length > 100)
            {
                _logger.LogWarning("Invalid service name length: {Length}", name.Length);
                return new BaseResponseModel<T> { Code = 400, Message = "Service name must be between 2 and 100 characters", Data = default };
            }
            if (price < 0)
            {
                _logger.LogWarning("Negative price provided: {Price}", price);
                return new BaseResponseModel<T> { Code = 400, Message = "Price cannot be negative", Data = default };
            }
            if (manufacturerId < 0)
            {
                _logger.LogWarning("Invalid Manufacturer ID: {Id}", manufacturerId);
                return new BaseResponseModel<T> { Code = 400, Message = "Manufacturer ID must be greater than 0", Data = default };
            }
            return null;
        }

        // Update to include validation for Description and Component
        private BaseResponseModel<T>? ValidateServiceInput<T>(string name, string description, string component, float price, long manufacturerId = 0)
        {
            var baseValidation = ValidateServiceInput<T>(name, price, manufacturerId);
            if (baseValidation != null) return baseValidation;

            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogWarning("Service description is required");
                return new BaseResponseModel<T> { Code = 400, Message = "Service description is required", Data = default };
            }
            if (description.Length > 500)
            {
                _logger.LogWarning("Invalid service description length: {Length}", description.Length);
                return new BaseResponseModel<T> { Code = 400, Message = "Service description must not exceed 500 characters", Data = default };
            }

            if (string.IsNullOrWhiteSpace(component))
            {
                _logger.LogWarning("Service component is required");
                return new BaseResponseModel<T> { Code = 400, Message = "Service component is required", Data = default };
            }
            if (component.Length > 100)
            {
                _logger.LogWarning("Invalid service component length: {Length}", component.Length);
                return new BaseResponseModel<T> { Code = 400, Message = "Service component must not exceed 100 characters", Data = default };
            }

            return null;
        }
    }
}