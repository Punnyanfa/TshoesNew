using FCSP.Models.Context;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Post
{
    public class PostRepository : GenericRepository<Models.Entities.Post>, IPostRepository
    {
        public PostRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Entities.Post>> GetPostsByUserIdAsync(long userId)
        {
            return await Entities
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
} 