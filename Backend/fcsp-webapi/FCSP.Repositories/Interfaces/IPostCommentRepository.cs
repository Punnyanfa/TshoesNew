using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IPostCommentRepository : IGenericRepository<PostComment>
    {
        Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(long postId);
        Task<IEnumerable<PostComment>> GetCommentsByUserIdAsync(long userId);
    }
}