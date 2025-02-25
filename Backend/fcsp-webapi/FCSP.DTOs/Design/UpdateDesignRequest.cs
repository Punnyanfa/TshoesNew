namespace FCSP.DTOs.Design
{
    public class UpdateDesignRequest
    {
        public long Id { get; set; }

        public long? CustomShoeDesignTemplateId { get; set; }

        public string? DesignData { get; set; }

        public string? Preview3DFileUrl { get; set; }

        public decimal? Price { get; set; }

        // New property for textures
        public List<long>? TextureIds { get; set; }
    }
}
