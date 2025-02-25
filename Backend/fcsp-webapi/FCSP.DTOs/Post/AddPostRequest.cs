namespace FCSP.DTOs.Post
{
    public class AddPostRequest
    {
        public long? UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
} 