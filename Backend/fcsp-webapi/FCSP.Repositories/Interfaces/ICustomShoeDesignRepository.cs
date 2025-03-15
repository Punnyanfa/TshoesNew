using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignRepository : IGenericRepository<CustomShoeDesign>
    {
        Task<IList<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId);
    }
}