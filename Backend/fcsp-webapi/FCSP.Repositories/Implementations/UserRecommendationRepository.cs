using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class UserRecommendationRepository : GenericRepository<UserRecommendation>, IUserRecommendationRepository
    {
        public UserRecommendationRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRecommendation>> GetRecommendationsByUserIdAsync(long userId)
        {
            return await _context.Set<UserRecommendation>()
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
} 