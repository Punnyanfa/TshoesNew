using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IManufacturerCriteriaRepository : IGenericRepository<ManufacturerCriteria>
    {
        Task<IEnumerable<ManufacturerCriteria>> GetByManufacturerIdAsync(long manufacturerId);
        Task<IEnumerable<ManufacturerCriteria>> GetByCriteriaIdAsync(long criteriaId);
    }
}