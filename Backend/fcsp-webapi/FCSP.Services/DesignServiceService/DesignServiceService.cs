using FCSP.DTOs;
using FCSP.DTOs.DesignService;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ServiceService;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FCSP.Services.DesignServiceService
{
    public class DesignServiceService : IDesignServiceService
    {
        private readonly IDesignServiceRepository _designServiceRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceService _serviceService;
        private readonly ILogger<DesignServiceService> _logger;

        public DesignServiceService(
            IDesignServiceRepository designServiceRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IServiceRepository serviceRepository,
            IServiceService serviceService,
            ILogger<DesignServiceService> logger)
        {
            _designServiceRepository = designServiceRepository ?? throw new ArgumentNullException(nameof(designServiceRepository));
            _customShoeDesignRepository = customShoeDesignRepository ?? throw new ArgumentNullException(nameof(customShoeDesignRepository));
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _serviceService = serviceService ?? throw new ArgumentNullException(nameof(serviceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BaseResponseModel<List<DesignServiceDto>>> GetAllDesignServices()
        {
            try
            {
                _logger.LogInformation("Fetching all design services");
                var designServices = await _designServiceRepository.GetAllAsync();

                // Cập nhật giá cho từng DesignService
                var designServiceDtos = new List<DesignServiceDto>();
                foreach (var designService in designServices)
                {
                    var price = await _serviceService.GetServicePriceAsync(designService.ServiceId);
                    if (price != designService.Price)
                    {
                        designService.Price = price;
                        designService.UpdatedAt = DateTime.UtcNow;
                        await _designServiceRepository.UpdateAsync(designService);
                    }
                    designServiceDtos.Add(MapToDto(designService));
                }

                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all design services");
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
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid DesignService ID: {Id}", request.Id);
                    return new BaseResponseModel<GetDesignServiceByIdResponse>
                    {
                        Code = 400,
                        Message = "DesignService ID must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Fetching design service with ID: {Id}", request.Id);
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    _logger.LogWarning("Design service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<GetDesignServiceByIdResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                // Cập nhật giá từ Service
                var price = await _serviceService.GetServicePriceAsync(designService.ServiceId);
                if (price != designService.Price)
                {
                    designService.Price = price;
                    designService.UpdatedAt = DateTime.UtcNow;
                    await _designServiceRepository.UpdateAsync(designService);
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
                _logger.LogError(ex, "Error fetching design service with ID: {Id}", request.Id);
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
                // Validate request
                if (request.DesignId <= 0 || request.ServiceId <= 0)
                {
                    _logger.LogWarning("Invalid DesignId: {DesignId} or ServiceId: {ServiceId}", request.DesignId, request.ServiceId);
                    return new BaseResponseModel<AddDesignServiceResponse>
                    {
                        Code = 400,
                        Message = "DesignId and ServiceId must be greater than 0",
                        Data = null
                    };
                }

                // Kiểm tra CustomShoeDesign tồn tại
                _logger.LogInformation("Checking CustomShoeDesign with ID: {DesignId}", request.DesignId);
                var customShoeDesign = await _customShoeDesignRepository.FindAsync((object)request.DesignId);
                if (customShoeDesign == null)
                {
                    _logger.LogWarning("CustomShoeDesign not found for ID: {DesignId}", request.DesignId);
                    return new BaseResponseModel<AddDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "CustomShoeDesign not found",
                        Data = null
                    };
                }

                // Kiểm tra Service tồn tại
                _logger.LogInformation("Checking Service with ID: {ServiceId}", request.ServiceId);
                var service = await _serviceRepository.FindAsync(request.ServiceId);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {ServiceId}", request.ServiceId);
                    return new BaseResponseModel<AddDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                // Lấy giá của Service
                _logger.LogInformation("Fetching price for Service with ID: {ServiceId}", request.ServiceId);
                var servicePrice = await _serviceService.GetServicePriceAsync(request.ServiceId);
                if (servicePrice == null)
                {
                    _logger.LogWarning("No active price found for Service with ID: {ServiceId}", request.ServiceId);
                    return new BaseResponseModel<AddDesignServiceResponse>
                    {
                        Code = 400,
                        Message = "No active price found for the specified Service",
                        Data = null
                    };
                }

                // Tạo DesignService mới
                _logger.LogInformation("Adding new design service for DesignId: {DesignId}, ServiceId: {ServiceId}", request.DesignId, request.ServiceId);
                var designService = new DesignService
                {
                    CustomShoeDesignId = request.DesignId,
                    ServiceId = request.ServiceId,
                    Price = servicePrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedDesignService = await _designServiceRepository.AddAsync(designService);

                _logger.LogInformation("Design service added successfully with ID: {DesignServiceId}", addedDesignService.Id);
                return new BaseResponseModel<AddDesignServiceResponse>
                {
                    Code = 201,
                    Message = "Design service added successfully",
                    Data = new AddDesignServiceResponse
                    {
                        DesignServiceId = addedDesignService.Id,
                        Price = addedDesignService.Price
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding design service for DesignId: {DesignId}, ServiceId: {ServiceId}", request.DesignId, request.ServiceId);
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
                if (request.Id <= 0 || request.DesignId <= 0 || request.ServiceId <= 0)
                {
                    _logger.LogWarning("Invalid Id: {Id}, DesignId: {DesignId}, or ServiceId: {ServiceId}", request.Id, request.DesignId, request.ServiceId);
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 400,
                        Message = "Id, DesignId, and ServiceId must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Updating design service with ID: {Id}", request.Id);
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    _logger.LogWarning("Design service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                // Kiểm tra CustomShoeDesign tồn tại
                _logger.LogInformation("Checking CustomShoeDesign with ID: {DesignId}", request.DesignId);
                var customShoeDesign = await _customShoeDesignRepository.FindAsync((object)request.DesignId);
                if (customShoeDesign == null)
                {
                    _logger.LogWarning("CustomShoeDesign not found for ID: {DesignId}", request.DesignId);
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "CustomShoeDesign not found",
                        Data = null
                    };
                }

                // Kiểm tra Service tồn tại
                _logger.LogInformation("Checking Service with ID: {ServiceId}", request.ServiceId);
                var service = await _serviceRepository.FindAsync(request.ServiceId);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {ServiceId}", request.ServiceId);
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Service not found",
                        Data = null
                    };
                }

                // Lấy giá của Service
                _logger.LogInformation("Fetching price for Service with ID: {ServiceId}", request.ServiceId);
                var servicePrice = await _serviceService.GetServicePriceAsync(request.ServiceId);
                if (servicePrice == null)
                {
                    _logger.LogWarning("No active price found for Service with ID: {ServiceId}", request.ServiceId);
                    return new BaseResponseModel<UpdateDesignServiceResponse>
                    {
                        Code = 400,
                        Message = "No active price found for the specified Service",
                        Data = null
                    };
                }

                // Cập nhật DesignService
                designService.CustomShoeDesignId = request.DesignId;
                designService.ServiceId = request.ServiceId;
                designService.Price = servicePrice;
                designService.UpdatedAt = DateTime.UtcNow;

                await _designServiceRepository.UpdateAsync(designService);

                _logger.LogInformation("Design service updated successfully with ID: {DesignServiceId}", designService.Id);
                return new BaseResponseModel<UpdateDesignServiceResponse>
                {
                    Code = 200,
                    Message = "Design service updated successfully",
                    Data = new UpdateDesignServiceResponse
                    {
                        DesignServiceId = designService.Id,
                        Price = designService.Price
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating design service with ID: {Id}", request.Id);
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
                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid DesignService ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteDesignServiceResponse>
                    {
                        Code = 400,
                        Message = "DesignService ID must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Deleting design service with ID: {Id}", request.Id);
                var designService = await _designServiceRepository.FindAsync(request.Id);
                if (designService == null)
                {
                    _logger.LogWarning("Design service not found for ID: {Id}", request.Id);
                    return new BaseResponseModel<DeleteDesignServiceResponse>
                    {
                        Code = 404,
                        Message = "Design service not found",
                        Data = null
                    };
                }

                await _designServiceRepository.DeleteAsync(request.Id);

                _logger.LogInformation("Design service deleted successfully with ID: {Id}", request.Id);
                return new BaseResponseModel<DeleteDesignServiceResponse>
                {
                    Code = 200,
                    Message = "Design service deleted successfully",
                    Data = new DeleteDesignServiceResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting design service with ID: {Id}", request.Id);
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
                if (request.CustomShoeDesignId <= 0)
                {
                    _logger.LogWarning("Invalid CustomShoeDesignId: {CustomShoeDesignId}", request.CustomShoeDesignId);
                    return new BaseResponseModel<List<DesignServiceDto>>
                    {
                        Code = 400,
                        Message = "CustomShoeDesignId must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Fetching design services for CustomShoeDesignId: {CustomShoeDesignId}", request.CustomShoeDesignId);
                var designServices = await _designServiceRepository.GetDesignServicesByCustomShoeDesignId(request.CustomShoeDesignId);

                // Cập nhật giá cho từng DesignService
                var designServiceDtos = new List<DesignServiceDto>();
                foreach (var designService in designServices)
                {
                    var price = await _serviceService.GetServicePriceAsync(designService.ServiceId);
                    if (price != designService.Price)
                    {
                        designService.Price = price;
                        designService.UpdatedAt = DateTime.UtcNow;
                        await _designServiceRepository.UpdateAsync(designService);
                    }
                    designServiceDtos.Add(MapToDto(designService));
                }

                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching design services for CustomShoeDesignId: {CustomShoeDesignId}", request.CustomShoeDesignId);
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
                if (request.ServiceId <= 0)
                {
                    _logger.LogWarning("Invalid ServiceId: {ServiceId}", request.ServiceId);
                    return new BaseResponseModel<List<DesignServiceDto>>
                    {
                        Code = 400,
                        Message = "ServiceId must be greater than 0",
                        Data = null
                    };
                }

                _logger.LogInformation("Fetching design services for ServiceId: {ServiceId}", request.ServiceId);
                var designServices = await _designServiceRepository.GetDesignServicesByServiceId(request.ServiceId);

                // Cập nhật giá cho từng DesignService
                var designServiceDtos = new List<DesignServiceDto>();
                foreach (var designService in designServices)
                {
                    var price = await _serviceService.GetServicePriceAsync(designService.ServiceId);
                    if (price != designService.Price)
                    {
                        designService.Price = price;
                        designService.UpdatedAt = DateTime.UtcNow;
                        await _designServiceRepository.UpdateAsync(designService);
                    }
                    designServiceDtos.Add(MapToDto(designService));
                }

                return new BaseResponseModel<List<DesignServiceDto>>
                {
                    Code = 200,
                    Message = "Design services retrieved successfully",
                    Data = designServiceDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching design services for ServiceId: {ServiceId}", request.ServiceId);
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