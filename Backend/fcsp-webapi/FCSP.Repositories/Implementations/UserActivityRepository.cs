using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class UserActivityRepository : GenericRepository<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserActivity>> GetActivitiesByUserIdAsync(long userId)
        {
            return await Entities
                .Include(ua => ua.ViewedDesign)
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserActivity>> GetActivitiesByDesignIdAsync(long designId)
        {
            return await Entities
                .Include(ua => ua.User)
                .Where(ua => ua.ViewedDesignId == designId)
                .OrderByDescending(ua => ua.CreatedAt)
                .ToListAsync();
        }
    }
} 