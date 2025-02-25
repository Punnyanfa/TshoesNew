namespace FCSP.DTOs.CustomShoeDesignTexture
{
    public class GetCustomShoeDesignTextureByIdResponse : BaseResponse
    {
        public long? CustomShoeDesignId { get; set; }
        public long? TextureId { get; set; }
    }
} 