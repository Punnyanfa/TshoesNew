using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IDesignServiceRepository : IGenericRepository<DesignService>
    {
        Task<IEnumerable<DesignService>> GetServicesByCustomShoeDesignIdAsync(long customShoeDesignId);
        Task AddRangeAsync(IEnumerable<DesignService> entities);
        Task RemoveRangeAsync(IEnumerable<long> designIds);
    }
}