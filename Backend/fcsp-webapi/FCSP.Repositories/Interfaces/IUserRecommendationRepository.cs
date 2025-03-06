using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IUserRecommendationRepository : IGenericRepository<UserRecommendation>
    {
        Task<IEnumerable<UserRecommendation>> GetRecommendationsByUserIdAsync(long userId);
    }
} 