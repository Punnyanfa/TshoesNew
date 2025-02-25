namespace FCSP.DTOs.Post
{
    public class GetPostByIdResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CommentCount { get; set; }
    }
} 