using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Rating
{
    public class GetRatingByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetRatingByIdResponse
    {
        public long Id { get; set; }
        public string? DesignName { get; set; }
        public string? DesignPreviewUrl { get; set; }
        public string? UserName { get; set; }
        public int UserRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AddRatingRequest
    {
        public long UserId { get; set; }
        public long CustomShoeDesignId { get; set; } // Đổi từ TargetId

        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5")]
        public int Value { get; set; }

        [MaxLength(250, ErrorMessage = "Comment cannot exceed 250 characters")]
        [BadWordValidation(ErrorMessage = "Comment contains inappropriate language")]
        public string Comment { get; set; }
    }

    public class AddRatingResponse
    {
        public long RatingId { get; set; }
    }

    public class UpdateRatingRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CustomShoeDesignId { get; set; } // Đổi từ TargetId
        public string Type { get; set; }

        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5")]
        public int Value { get; set; }

        [MaxLength(250, ErrorMessage = "Comment cannot exceed 250 characters")]
        [BadWordValidation(ErrorMessage = "Comment contains inappropriate language")]
        public string Comment { get; set; }
    }

    public class UpdateRatingResponse
    {
        public long RatingId { get; set; }
    }

    public class DeleteRatingRequest
    {
        public long Id { get; set; }
    }

    public class DeleteRatingResponse
    {
        public bool Success { get; set; }
    }

    public class CustomShoeRatingStats
    {
        public long CustomShoeDesignId { get; set; }
        public Dictionary<int, int> RatingBreakdown { get; set; } = new Dictionary<int, int>();
    }

    public class TopRatedCustomShoe
    {
        public long CustomShoeDesignId { get; set; }
        public int HighRatingCount { get; set; }
    }
}
