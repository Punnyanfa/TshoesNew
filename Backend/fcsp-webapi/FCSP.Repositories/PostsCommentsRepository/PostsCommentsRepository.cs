using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations;

public class PostsCommentsRepository : GenericRepository<PostsComments>, IPostsCommentsRepository
{
    public PostsCommentsRepository(FcspDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PostsComments>> GetCommentsByPostIdAsync(long postId)
    {
        return await _context.Set<PostsComments>()
            .Where(pc => pc.PostsId == postId && !pc.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<PostsComments>> GetCommentsByUserIdAsync(long userId)
    {
        return await _context.Set<PostsComments>()
            .Where(pc => pc.UserId == userId && !pc.IsDeleted)
            .ToListAsync();
    }
}