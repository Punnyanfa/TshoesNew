namespace FCSP.DTOs.Rating
{
    public class GetRatingByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetRatingByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TargetId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddRatingRequest
    {
        public long UserId { get; set; }
        public long TargetId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
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
        public long TargetId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
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
} 