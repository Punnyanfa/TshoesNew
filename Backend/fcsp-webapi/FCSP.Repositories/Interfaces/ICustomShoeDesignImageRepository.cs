using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTexturesRepository : IGenericRepository<CustomShoeDesignTextures>
    {
        Task<IEnumerable<CustomShoeDesignTextures>> GetByCustomShoeDesignIdAsync(long customShoeDesignId);
    }
}