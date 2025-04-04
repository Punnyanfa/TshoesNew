using FCSP.DTOs;
using FCSP.DTOs.DesignService;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.DesignServiceService
{
    public class DesignServiceService : IDesignServiceService
    {
        private readonly IDesignServiceRepository _designServiceRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IServiceRepository _serviceRepository;

        public DesignServiceService(
            IDesignServiceRepository designServiceRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IServiceRepository serviceRepository)
        {
            _designServiceRepository = designServiceRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<BaseResponseModel<List<DesignServiceDto>>> GetAllDesignServices()
        {
            try
            {
                var designServices = await _designServiceRepository.GetAllAsync();
                var designServiceDtos = designServices.Select(MapToDto).ToList();
                
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetDesignServiceByIdResponse>> GetDesignServiceById(GetDesignServiceByIdRequest request)
        {
            try
            {
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    return new BaseResponseModel<GetDesignServiceByIdResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                string? customShoeDesignName = null;
                string? serviceName = null;

                if (designService.CustomShoeDesign != null)
                {
                    customShoeDesignName = $"Design #{designService.CustomShoeDesign.Id}";
                }

                if (designService.Service != null)
                {
                    serviceName = designService.Service.ServiceName;
                }

                return new BaseResponseModel<GetDesignServiceByIdResponse>
                {
                    Code = 200,
                    Message = "Design service retrieved successfully",
                    Data = new GetDesignServiceByIdResponse
                    {
                        Id = designService.Id,
                        DesignId = designService.CustomShoeDesignId,
                        ServiceId = designService.ServiceId,
                        Price = designService.Price,
                        CustomShoeDesignName = customShoeDesignName,
                        ServiceName = serviceName
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetDesignServiceByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddDesignServiceResponse>> AddDesignService(AddDesignServiceRequest request)
        {
            try
            {
                var designService = new DesignService
                {
                    CustomShoeDesignId = request.DesignId,
                    ServiceId = request.ServiceId,
                    Price = request.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedDesignService = await _designServiceRepository.AddAsync(designService);
                
                return new BaseResponseModel<AddDesignServiceResponse>
                {
                    Code = 201,
                    Message = "Design service added successfully",
                    Data = new AddDesignServiceResponse { DesignServiceId = addedDesignService.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddDesignServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateDesignServiceResponse>> UpdateDesignService(UpdateDesignServiceRequest request)
        {
            try
            {
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                designService.CustomShoeDesignId = request.DesignId;
                designService.ServiceId = request.ServiceId;
                designService.Price = request.Price;
                designService.UpdatedAt = DateTime.UtcNow;

                await _designServiceRepository.UpdateAsync(designService);
                
                return new BaseResponseModel<UpdateDesignServiceResponse>
                {
                    Code = 200,
                    Message = "Design service updated successfully",
                    Data = new UpdateDesignServiceResponse { DesignServiceId = designService.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateDesignServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteDesignServiceResponse>> DeleteDesignService(DeleteDesignServiceRequest request)
        {
            try
            {
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    return new BaseResponseModel<DeleteDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                await _designServiceRepository.DeleteAsync(request.Id);
                
                return new BaseResponseModel<DeleteDesignServiceResponse>
                {
                    Code = 200,
                    Message = "Design service deleted successfully",
                    Data = new DeleteDesignServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteDesignServiceResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<DesignServiceDto>>> GetDesignServicesByCustomShoeDesignId(GetDesignServicesByCustomShoeDesignIdRequest request)
        {
            try
            {
                var designServices = await _designServiceRepository.GetDesignServicesByCustomShoeDesignId(request.CustomShoeDesignId);
                var designServiceDtos = designServices.Select(MapToDto).ToList();
                
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<DesignServiceDto>>> GetDesignServicesByServiceId(GetDesignServicesByServiceIdRequest request)
        {
            try
            {
                var designServices = await _designServiceRepository.GetDesignServicesByServiceId(request.ServiceId);
                var designServiceDtos = designServices.Select(MapToDto).ToList();
                
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private DesignServiceDto MapToDto(DesignService designService)
        {
            return new DesignServiceDto
            {
                Id = designService.Id,
                CustomShoeDesignId = designService.CustomShoeDesignId,
                ServiceId = designService.ServiceId,
                Price = designService.Price,
                CreatedAt = designService.CreatedAt,
                UpdatedAt = designService.UpdatedAt
            };
        }
    }
} 