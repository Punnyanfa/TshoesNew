using FCSP.Models.Entities;

namespace FCSP.Repositories.PostComment
{
    public interface IPostCommentRepository : IGenericRepository<Models.Entities.PostComment>
    {
        Task<IEnumerable<Models.Entities.PostComment>> GetCommentsByPostIdAsync(long postId);
        Task<IEnumerable<Models.Entities.PostComment>> GetCommentsByUserIdAsync(long userId);
    }
} 