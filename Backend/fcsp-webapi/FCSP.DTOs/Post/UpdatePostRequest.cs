namespace FCSP.DTOs.Post
{
    public class UpdatePostRequest
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
} 