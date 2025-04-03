using FCSP.DTOs;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<BaseResponseModel<List<Service>>> GetAllServices()
        {
            try
            {
                var services = await _serviceRepository.GetAllAsync();
                return new BaseResponseModel<List<Service>>
                {
                    Code = 200,
                    Message = "Services retrieved successfully",
                    Data = services.ToList()
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<Service>>
                {
                    Code = 500,
                    Message = $"Error retrieving services: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<GetServiceByIdResponse>> GetServiceById(GetServiceByIdRequest request)
        {
            try
            {
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<GetServiceByIdResponse>
                    {
                        Code = 404,
                        Message = "Service not found"
                    };
                }

                var response = new GetServiceByIdResponse
                {
                    Id = service.Id,
                    Name = service.ServiceName,
                    Price = service.SetServiceAmounts.FirstOrDefault()?.Amount ?? 0,
                    IsDeleted = service.IsDeleted
                };

                return new BaseResponseModel<GetServiceByIdResponse>
                {
                    Code = 200,
                    Message = "Service retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetServiceByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving service: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddServiceResponse>> AddService(AddServiceRequest request)
        {
            try
            {
                var service = new Service
                {
                    ServiceName = request.Name,
                    IsDeleted = false
                };

                var addedService = await _serviceRepository.AddAsync(service);
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 201,
                    Message = "Service created successfully",
                    Data = new AddServiceResponse { ServiceId = addedService.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddServiceResponse>
                {
                    Code = 500,
                    Message = $"Error creating service: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<UpdateServiceResponse>> UpdateService(UpdateServiceRequest request)
        {
            try
            {
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<UpdateServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found"
                    };
                }

                service.ServiceName = request.Name;

                await _serviceRepository.UpdateAsync(service);
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 200,
                    Message = "Service updated successfully",
                    Data = new UpdateServiceResponse { ServiceId = service.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateServiceResponse>
                {
                    Code = 500,
                    Message = $"Error updating service: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<DeleteServiceResponse>> DeleteService(DeleteServiceRequest request)
        {
            try
            {
                var service = await _serviceRepository.FindAsync(request.Id);
                if (service == null)
                {
                    return new BaseResponseModel<DeleteServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found"
                    };
                }
                
                service.IsDeleted = true;
                await _serviceRepository.UpdateAsync(service);
                
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 200,
                    Message = "Service deleted successfully",
                    Data = new DeleteServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteServiceResponse>
                {
                    Code = 500,
                    Message = $"Error deleting service: {ex.Message}"
                };
            }
        }
    }
}