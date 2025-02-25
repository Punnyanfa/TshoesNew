namespace FCSP.DTOs.Texture
{
    public class AddTextureResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Prompt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 