using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IPostRepository : IGenericRepository<Posts>
    {
        Task<IEnumerable<Posts>> GetPostsByUserIdAsync(long userId);
    }
} 