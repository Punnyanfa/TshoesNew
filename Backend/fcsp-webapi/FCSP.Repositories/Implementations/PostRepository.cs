using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
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