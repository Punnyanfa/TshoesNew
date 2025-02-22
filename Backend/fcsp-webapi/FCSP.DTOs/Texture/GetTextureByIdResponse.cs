namespace FCSP.DTOs.Texture
{
    public class GetTextureByIdResponse
    {
        public long? UserId { get; set; }

        public int? Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Prompt { get; set; }
    }
}
