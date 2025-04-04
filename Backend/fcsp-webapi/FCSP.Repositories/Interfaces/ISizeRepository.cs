using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ISizeRepository : IGenericRepository<Size>
    {
        Task<Size> GetSizeEntityBySizeValueAsync(int sizeValue);
    }
} 