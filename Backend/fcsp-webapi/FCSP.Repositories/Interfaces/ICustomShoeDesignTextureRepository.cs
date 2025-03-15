using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTextureRepository : IGenericRepository<CustomShoeDesignTexture>
    {
        Task AddRangeAsync(IEnumerable<CustomShoeDesignTexture> entities);
        Task DeleteByDesignIdAsync(long designId);
    }
}