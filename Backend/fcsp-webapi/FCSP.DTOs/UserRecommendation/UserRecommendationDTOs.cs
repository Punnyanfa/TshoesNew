using FCSP.DTOs;

namespace FCSP.DTOs.UserRecommendation
{
    public class UserRecommendationDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CustomShoeDesignId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetUserRecommendationsByUserIdRequest
    {
        public long UserId { get; set; }
    }

    public class GetUserRecommendationsByUserIdResponse
    {
        public List<UserRecommendationDto> Recommendations { get; set; } = new List<UserRecommendationDto>();
    }

    public class AddUserRecommendationRequest
    {
        public long UserId { get; set; }
        public long CustomShoeDesignId { get; set; }
    }

    public class AddUserRecommendationResponse
    {
        public bool Success { get; set; }
        public long Id { get; set; }
    }

    public class DeleteUserRecommendationRequest
    {
        public long Id { get; set; }
    }

    public class DeleteUserRecommendationResponse
    {
        public bool Success { get; set; }
    }
} 