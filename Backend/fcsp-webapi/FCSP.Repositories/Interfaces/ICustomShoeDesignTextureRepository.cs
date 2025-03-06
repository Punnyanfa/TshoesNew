using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignTextureRepository : IGenericRepository<CustomShoeDesignTexture>
    {
        Task AddRangeAsync(IEnumerable<CustomShoeDesignTexture> entities);
        Task DeleteByDesignIdAsync(long designId);
    }
}