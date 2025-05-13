using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;

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

                var result = services.Select(s =>
                {
                    return new ServiceResponseDto
                    {
                        Id = s.Id,
                        Component = s.Component,
                        Type = s.Type,
                        Price = s.Price,
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

                var result = services.Select(s =>
                {
                    return new ServiceResponseDto
                    {
                        Id = s.Id,
                        Component = s.Component,
                        Type = s.Type,
                        Price = s.Price,
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
                var services = GetServicesFromAddServiceRequest(request);

                if (services.Any())
                {
                    await _serviceRepository.AddRangeAsync(services);
                }

                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 201,
                    Message = "Success",
                    Data = new AddServiceResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new AddServiceResponse
                    {
                        Success = false
                    }
                };
            }
        }

        public async Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request)
        {
            try
            {
                var service = await _serviceRepository.GetServiceWithDetailsAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<UpdateServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                service.Component = request.Component ?? service.Component;
                service.Type = request.Type ?? service.Type;
                if (request.Price > 0)
                {
                    service.Price = request.Price;
                }

                await _serviceRepository.UpdateAsync(service);
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 200,
                    Message = "Success",
                    Data = new UpdateServiceResponse
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
                    Data = new DeleteServiceResponse
                    {
                        Success = true
                    }
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
        public IEnumerable<Service> GetServicesFromAddServiceRequest(AddServiceRequest request)
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