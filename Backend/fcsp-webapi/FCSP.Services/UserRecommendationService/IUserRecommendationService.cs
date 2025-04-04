using FCSP.DTOs;
using FCSP.DTOs.UserRecommendation;
using FCSP.Models.Entities;

namespace FCSP.Services.UserRecommendationService
{
    public interface IUserRecommendationService
    {
        Task<BaseResponseModel<GetUserRecommendationsByUserIdResponse>> GetRecommendationsByUserId(GetUserRecommendationsByUserIdRequest request);
        Task<BaseResponseModel<AddUserRecommendationResponse>> AddRecommendation(AddUserRecommendationRequest request);
        Task<BaseResponseModel<DeleteUserRecommendationResponse>> DeleteRecommendation(DeleteUserRecommendationRequest request);
    }
} 