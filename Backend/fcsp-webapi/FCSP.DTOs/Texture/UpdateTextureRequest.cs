namespace FCSP.DTOs.Texture
{
    public class UpdateTextureRequest
    {
        public long Id { get; set; }
        public int? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Prompt { get; set; }
    }
}
