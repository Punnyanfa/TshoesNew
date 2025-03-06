using FCSP.Models.Entities;

namespace FCSP.Services.UserRecommendationService
{
    public interface IUserRecommendationService
    {
        Task<IEnumerable<UserRecommendation>> GetRecommendationsByUserId(long userId);
        Task<UserRecommendation> AddRecommendation(UserRecommendation recommendation);
        Task<bool> DeleteRecommendation(long id);
    }
} 