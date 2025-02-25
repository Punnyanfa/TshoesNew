using FCSP.Models.Context;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.PostComment
{
    public class PostCommentRepository : GenericRepository<Models.Entities.PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Entities.PostComment>> GetCommentsByPostIdAsync(long postId)
        {
            return await Entities
                .Where(pc => pc.PostId == postId)
                .OrderByDescending(pc => pc.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Entities.PostComment>> GetCommentsByUserIdAsync(long userId)
        {
            return await Entities
                .Where(pc => pc.UserId == userId)
                .OrderByDescending(pc => pc.CreatedAt)
                .ToListAsync();
        }
    }
} 