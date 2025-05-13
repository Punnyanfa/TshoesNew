using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IDesignerRepository : IGenericRepository<Designer>
    {
        Task<Designer?> GetDesignerByUserIdAsync(long userId);
        Task<IEnumerable<Designer>> GetActiveDesignersAsync();
    }
}