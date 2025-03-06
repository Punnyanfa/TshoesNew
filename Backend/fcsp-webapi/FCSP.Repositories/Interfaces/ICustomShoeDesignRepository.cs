using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface ICustomShoeDesignRepository : IGenericRepository<CustomShoeDesign>
    {
        Task<IList<CustomShoeDesign>> GetDesignsByUserIdAsync(long userId);
    }
}