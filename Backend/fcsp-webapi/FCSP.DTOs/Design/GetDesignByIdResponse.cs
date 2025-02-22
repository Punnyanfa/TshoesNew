namespace FCSP.DTOs.Design
{
    public class GetDesignByIdResponse
    {
        public long? UserId { get; set; }

        public long? CustomShoeDesignTemplateId { get; set; }

        public string? DesignData { get; set; }

        public string? Preview3DFileUrl { get; set; }

        public decimal? Price { get; set; }
    }
}
