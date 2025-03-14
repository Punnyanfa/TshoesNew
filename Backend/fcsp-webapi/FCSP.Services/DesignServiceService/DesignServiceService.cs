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
                DesignId = designService.CustomShoeDesignId,
                ServiceId = designService.ServiceId,
                Price = designService.Price,
                CustomShoeDesignName = customShoeDesignName,
                ServiceName = serviceName
            };
        }

        public async Task<AddDesignServiceResponse> AddDesignService(AddDesignServiceRequest request)
        {
            var designService = new DesignService
            {
                CustomShoeDesignId = request.DesignId,
                ServiceId = request.ServiceId,
                Price = request.Price
            };

            var addedDesignService = await _designServiceRepository.AddAsync(designService);
            return new AddDesignServiceResponse { DesignServiceId = addedDesignService.Id };
        }

        public async Task<UpdateDesignServiceResponse> UpdateDesignService(UpdateDesignServiceRequest request)
        {
            var designService = await _designServiceRepository.FindAsync(request.Id);
            if (designService == null)
                throw new Exception("Design service not found");

            designService.CustomShoeDesignId = request.DesignId;
            designService.ServiceId = request.ServiceId;
            designService.Price = request.Price;

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

        public async Task<IEnumerable<DesignService>> GetDesignServicesByServiceId(long serviceId)
        {
            return await _designServiceRepository.GetDesignServicesByServiceId(serviceId);
        }
    }
} 