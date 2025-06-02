using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTexturesRepository : IGenericRepository<CustomShoeDesignTextures>
    {
        Task<IEnumerable<CustomShoeDesignTextures>> GetByCustomShoeDesignIdAsync(long customShoeDesignId);
        Task RemoveRangeAsync(IEnumerable<long> designIds);
        Task AddRangeAsync(IEnumerable<CustomShoeDesignTextures> entities);
    }
}