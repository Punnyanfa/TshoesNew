using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IManufacturerRepository : IGenericRepository<Manufacturer>
    {
        Task<IEnumerable<Manufacturer>> GetManufacturersByStatusAsync(int status);
        Task<Manufacturer?> GetManufacturerByUserIdAsync(long userId);
        Task<Manufacturer> GetManufacturerWithDetailsAsync(long id);
        Task<List<Manufacturer>> GetAllWithDetailsAsync();
    }
}