using FCSP.Models.Entities;
using FCSP.Repositories.Implementations;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.UserRecommendationService
{
    public class UserRecommendationService : IUserRecommendationService
    {
        private readonly IUserRecommendationRepository _userRecommendationRepository;

        public UserRecommendationService(IUserRecommendationRepository userRecommendationRepository)
        {
            _userRecommendationRepository = userRecommendationRepository;
        }

        public async Task<IEnumerable<UserRecommendation>> GetRecommendationsByUserId(long userId)
        {
            var recommendations = await _userRecommendationRepository.GetAllAsync();
            return recommendations.Where(r => r.UserId == userId);
        }

        public async Task<UserRecommendation> AddRecommendation(UserRecommendation recommendation)
        {
            return await _userRecommendationRepository.AddAsync(recommendation);
        }

        public async Task<bool> DeleteRecommendation(long id)
        {
            await _userRecommendationRepository.DeleteAsync(id);
            return true;
        }
    }
} 