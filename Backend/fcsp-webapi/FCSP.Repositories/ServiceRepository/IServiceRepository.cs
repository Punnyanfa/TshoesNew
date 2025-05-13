using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IList<Service>> GetByManufacturerIdAsync(long manufacturerId);
        Task<IList<Service>> GetActiveServicesAsync();
        Task<Service> GetServiceWithDetailsAsync(long id);
    }
}