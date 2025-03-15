using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ISetServiceAmountRepository : IGenericRepository<SetServiceAmount>
    {
        Task<IEnumerable<SetServiceAmount>> GetCurrentAmountsByServiceIdAsync(long serviceId);
        Task<SetServiceAmount?> GetActiveAmountByServiceIdAsync(long serviceId);
    }
}