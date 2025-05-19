using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;

        public ServiceService(
            IServiceRepository serviceRepository,
            IManufacturerRepository manufacturerRepository,
            IUserRepository userRepository)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<BaseResponseModel<List<ServiceResponseDto>>> GetAllServices()
        {
            try
            {
                var services = await _serviceRepository.GetActiveServicesAsync();
                if (services == null || !services.Any())
                {
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 404,
                        Message = "No active services found",
                        Data = null
                    };
                }

                var result = services.Select(s => new ServiceResponseDto
                {
                    Id = s.Id,
                    Component = s.Component,
                    Type = s.Type,
                    Price = s.Price,
                    ManufacturerId = s.ManufacturerId,
                    IsDeleted = s.IsDeleted
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
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 400,
                        Message = "Manufacturer ID must be greater than 0",
                        Data = null
                    };
                }

                var services = await _serviceRepository.GetByManufacturerIdAsync(manufacturerId);
                if (services == null || !services.Any())
                {
                    return new BaseResponseModel<List<ServiceResponseDto>>
                    {
                        Code = 404,
                        Message = "No services found for this manufacturer",
                        Data = null
                    };
                }

                var result = services.Select(s => new ServiceResponseDto
                {
                    Id = s.Id,
                    Component = s.Component,
                    Type = s.Type,
                    Price = s.Price,
                    ManufacturerId = s.ManufacturerId,
                    IsDeleted = s.IsDeleted
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
                    return new BaseResponseModel<ServiceResponseDto>
                    {
                        Code = 400,
                        Message = "Service ID must be greater than 0",
                        Data = null
                    };
                }

                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<ServiceResponseDto>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                return new BaseResponseModel<ServiceResponseDto>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new ServiceResponseDto
                    {
                        Id = service.Id,
                        Component = service.Component,
                        Type = service.Type,
                        Price = service.Price,
                        ManufacturerId = service.ManufacturerId,
                        IsDeleted = service.IsDeleted
                    }
                };
            }
            catch (Exception ex)
            {
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
                if (request.AddServices == null || !request.AddServices.Any())
                {
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 400,
                        Message = "No services to add",
                        Data = new AddServiceResponse { Success = false }
                    };
                }

                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.AddServices.First().ManufacturerId);
                if (manufacturer == null)
                {
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 404,
                        Message = "Manufacturer not found",
                        Data = new AddServiceResponse { Success = false }
                    };
                }

                var user = await _userRepository.GetByIdAsync(manufacturer.UserId);
                if (user == null || user.UserRole != UserRole.Manufacturer)
                {
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Only users with Manufacturer role can add services",
                        Data = new AddServiceResponse { Success = false }
                    };
                }

                if (manufacturer.Status != ManufacturerStatus.Active)
                {
                    return new BaseResponseModel<AddServiceResponse>
                    {
                        Code = 403,
                        Message = "Services can only be added by Manufacturers with Active status",
                        Data = new AddServiceResponse { Success = false }
                    };
                }

                var services = GetServicesFromAddServiceRequest(request);
                if (services.Any())
                {
                    await _serviceRepository.AddRangeAsync(services);
                }

                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 201,
                    Message = "Success",
                    Data = new AddServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new AddServiceResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request)
        {
            try
            {
                if (request.UpdateServices == null || !request.UpdateServices.Any())
                {
                    return new BaseResponseModel<UpdateServiceResponse>
                    {
                        Code = 400,
                        Message = "No services to update",
                        Data = new UpdateServiceResponse { Success = false }
                    };
                }

                var updatedServices = new List<Service>();
                foreach (var serviceUpdate in request.UpdateServices)
                {
                    if (serviceUpdate.Price < 0)
                    {
                        return new BaseResponseModel<UpdateServiceResponse>
                        {
                            Code = 400,
                            Message = "Price cannot be negative",
                            Data = new UpdateServiceResponse { Success = false }
                        };
                    }

                    var service = await _serviceRepository.GetServiceWithDetailsAsync(serviceUpdate.Id);
                    if (service == null)
                    {
                        return new BaseResponseModel<UpdateServiceResponse>
                        {
                            Code = 404,
                            Message = $"Service with ID {serviceUpdate.Id} not found",
                            Data = new UpdateServiceResponse { Success = false }
                        };
                    }

                 
                    service.Price = serviceUpdate.Price;
                   
                    await _serviceRepository.UpdateAsync(service);
                    updatedServices.Add(service);
                }

                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new UpdateServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new UpdateServiceResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return new BaseResponseModel<DeleteServiceResponse>
                    {
                        Code = 400,
                        Message = "Service ID must be greater than 0",
                        Data = null
                    };
                }

                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<DeleteServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                service.IsDeleted = true;
                await _serviceRepository.UpdateAsync(service);
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new DeleteServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        #region Private Methods
        private IEnumerable<Service> GetServicesFromAddServiceRequest(AddServiceRequest request)
        {
            if (request.AddServices == null || !request.AddServices.Any())
            {
                return new List<Service>();
            }

            return request.AddServices.Select(service => new Service
            {
                Component = service.Component,
                Type = service.Type,
                Price = service.Price,
                IsDeleted = false,
                ManufacturerId = service.ManufacturerId
            });
        }
        #endregion
    }
}