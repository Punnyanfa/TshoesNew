using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTextureRepository : IGenericRepository<CustomShoeDesignTexture>
    {
        Task AddRangeAsync(IEnumerable<CustomShoeDesignTexture> entities);
        Task RemoveRangeAsync(IEnumerable<long> designIds);
    }
}