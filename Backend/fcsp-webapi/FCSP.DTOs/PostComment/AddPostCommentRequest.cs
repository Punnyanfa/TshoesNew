namespace FCSP.DTOs.PostComment
{
    public class AddPostCommentRequest
    {
        public long? UserId { get; set; }
        public long? PostId { get; set; }
        public string? Comment { get; set; }
    }
} 