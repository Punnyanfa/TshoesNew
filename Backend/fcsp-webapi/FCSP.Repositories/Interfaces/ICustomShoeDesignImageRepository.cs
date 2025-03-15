using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignImageRepository : IGenericRepository<CustomShoeDesignImage>
    {
        Task<IEnumerable<CustomShoeDesignImage>> GetByCustomShoeDesignIdAsync(long customShoeDesignId);
    }
}