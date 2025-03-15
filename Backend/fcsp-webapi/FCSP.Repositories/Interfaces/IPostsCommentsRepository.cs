using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IPostsCommentsRepository : IGenericRepository<PostsComments>
    {
        Task<IEnumerable<PostsComments>> GetCommentsByPostIdAsync(long postId);
        Task<IEnumerable<PostsComments>> GetCommentsByUserIdAsync(long userId);
    }
}