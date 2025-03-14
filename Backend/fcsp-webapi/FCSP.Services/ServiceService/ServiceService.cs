using FCSP.DTOs.Service;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Service>> GetAllServices()
        {
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<GetServiceByIdResponse> GetServiceById(GetServiceByIdRequest request)
        {
            var service = await _serviceRepository.FindAsync(request.Id);
            if (service == null)
                throw new Exception("Service not found");

            return new GetServiceByIdResponse
            {
                Id = service.Id,
                Name = service.ServiceName,
                Price = service.SetServiceAmounts.FirstOrDefault()?.Amount ?? 0,
                IsDeleted = service.IsDeleted
            };
        }

        public async Task<AddServiceResponse> AddService(AddServiceRequest request)
        {
            var service = new Service
            {
                ServiceName = request.Name,

                IsDeleted = false
            };

            var addedService = await _serviceRepository.AddAsync(service);
            return new AddServiceResponse { ServiceId = addedService.Id };
        }

        public async Task<UpdateServiceResponse> UpdateService(UpdateServiceRequest request)
        {
            var service = await _serviceRepository.FindAsync(request.Id);
            if (service == null)
                throw new Exception("Service not found");

            service.ServiceName = request.Name;

            await _serviceRepository.UpdateAsync(service);
            return new UpdateServiceResponse { ServiceId = service.Id };
        }

        public async Task<DeleteServiceResponse> DeleteService(DeleteServiceRequest request)
        {
            var service = await _serviceRepository.FindAsync(request.Id);
            if (service == null)
                throw new Exception("Service not found");
                
            service.IsDeleted = true;
            await _serviceRepository.UpdateAsync(service);
            
            return new DeleteServiceResponse { Success = true };
        }
    }
}