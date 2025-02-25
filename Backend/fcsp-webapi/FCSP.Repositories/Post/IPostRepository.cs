using FCSP.Models.Entities;

namespace FCSP.Repositories.Post
{
    public interface IPostRepository : IGenericRepository<Models.Entities.Post>
    {
        Task<IEnumerable<Models.Entities.Post>> GetPostsByUserIdAsync(long userId);
    }
} 