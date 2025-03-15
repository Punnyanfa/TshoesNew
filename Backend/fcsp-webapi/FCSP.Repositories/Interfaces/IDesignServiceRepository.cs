using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IDesignServiceRepository : IGenericRepository<DesignService>
    {
        // Add any custom repository methods here
        Task<IEnumerable<DesignService>> GetDesignServicesByCustomShoeDesignId(long customShoeDesignId);
        Task<IEnumerable<DesignService>> GetDesignServicesByServiceId(long serviceId);
    }
}