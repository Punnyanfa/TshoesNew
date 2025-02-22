namespace FCSP.DTOs.CustomShoeDesignTemplate
{
    public class AddTemplateRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public decimal? Price { get; set; } = decimal.Zero;

        public string? ImageUrl { get; set; } = string.Empty;
    }
}
