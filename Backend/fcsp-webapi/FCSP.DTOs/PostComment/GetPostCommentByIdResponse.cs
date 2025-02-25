namespace FCSP.DTOs.PostComment
{
    public class GetPostCommentByIdResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? PostId { get; set; }
        public string? Comment { get; set; }
        public string? UserName { get; set; }
        public string? PostTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 