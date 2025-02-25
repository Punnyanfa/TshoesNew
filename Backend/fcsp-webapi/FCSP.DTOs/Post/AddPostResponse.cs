namespace FCSP.DTOs.Post
{
    public class AddPostResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 