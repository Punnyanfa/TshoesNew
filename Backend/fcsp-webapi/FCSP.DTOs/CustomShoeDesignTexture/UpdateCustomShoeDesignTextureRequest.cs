namespace FCSP.DTOs.CustomShoeDesignTexture
{
    public class UpdateCustomShoeDesignTextureRequest
    {
        public long Id { get; set; }
        public long? CustomShoeDesignId { get; set; }
        public long? TextureId { get; set; }
    }
} 