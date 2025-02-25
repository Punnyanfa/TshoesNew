namespace FCSP.DTOs.PostComment
{
    public class AddPostCommentResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? PostId { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 