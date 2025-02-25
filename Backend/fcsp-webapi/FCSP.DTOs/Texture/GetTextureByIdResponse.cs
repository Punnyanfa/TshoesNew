namespace FCSP.DTOs.Texture
{
    public class GetTextureByIdResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Prompt { get; set; }
        public string? CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UsageCount { get; set; }
    }
}
