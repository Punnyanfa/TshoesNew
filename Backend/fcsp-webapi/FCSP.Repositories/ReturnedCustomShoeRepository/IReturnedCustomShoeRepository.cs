using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IReturnedCustomShoeRepository : IGenericRepository<ReturnedCustomShoe>
    {
        Task<IEnumerable<ReturnedCustomShoe>> GetByCustomShoeDesignIdAsync(long customShoeDesignId);
    }
}