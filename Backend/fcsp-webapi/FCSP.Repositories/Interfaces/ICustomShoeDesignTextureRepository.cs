using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTextureRepository : IGenericRepository<CustomShoeDesignTextures>
    {
        Task AddRangeAsync(IEnumerable<CustomShoeDesignTextures> entities);
        Task RemoveRangeAsync(IEnumerable<long> designIds);
    }
}