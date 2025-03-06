using FCSP.DTOs.DesignService;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

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

        public async Task<IEnumerable<DesignService>> GetAllDesignServices()
        {
            return await _designServiceRepository.GetAllAsync();
        }

        public async Task<GetDesignServiceByIdResponse> GetDesignServiceById(GetDesignServiceByIdRequest request)
        {
            var designService = await _designServiceRepository.FindAsync(request.Id);
            if (designService == null)
                throw new Exception("Design service not found");

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

            return new GetDesignServiceByIdResponse
            {
                Id = designService.Id,
                DesignId = designService.DesignId,
                ServiceId = designService.ServiceId,
                CustomShoeDesignId = designService.CustomShoeDesignIdNavigation,
                CustomShoeDesignName = customShoeDesignName,
                ServiceName = serviceName
            };
        }

        public async Task<AddDesignServiceResponse> AddDesignService(AddDesignServiceRequest request)
        {
            var designService = new DesignService
            {
                DesignId = request.DesignId,
                ServiceId = request.ServiceId,
                CustomShoeDesignIdNavigation = request.CustomShoeDesignId
            };

            var addedDesignService = await _designServiceRepository.AddAsync(designService);
            return new AddDesignServiceResponse { DesignServiceId = addedDesignService.Id };
        }

        public async Task<UpdateDesignServiceResponse> UpdateDesignService(UpdateDesignServiceRequest request)
        {
            var designService = await _designServiceRepository.FindAsync(request.Id);
            if (designService == null)
                throw new Exception("Design service not found");

            designService.DesignId = request.DesignId;
            designService.ServiceId = request.ServiceId;
            designService.CustomShoeDesignIdNavigation = request.CustomShoeDesignId;

            await _designServiceRepository.UpdateAsync(designService);
            return new UpdateDesignServiceResponse { DesignServiceId = designService.Id };
        }

        public async Task<DeleteDesignServiceResponse> DeleteDesignService(DeleteDesignServiceRequest request)
        {
            await _designServiceRepository.DeleteAsync(request.Id);
            return new DeleteDesignServiceResponse { Success = true };
        }

        public async Task<IEnumerable<DesignService>> GetDesignServicesByCustomShoeDesignId(long customShoeDesignId)
        {
            return await _designServiceRepository.GetDesignServicesByCustomShoeDesignId(customShoeDesignId);
        }

        public async Task<IEnumerable<DesignService>> GetDesignServicesByServiceId(float serviceId)
        {
            return await _designServiceRepository.GetDesignServicesByServiceId(serviceId);
        }
    }
} 