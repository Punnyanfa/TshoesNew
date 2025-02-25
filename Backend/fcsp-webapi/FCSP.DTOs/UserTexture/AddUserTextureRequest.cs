namespace FCSP.DTOs.UserTexture
{
    public class AddUserTextureRequest
    {
        public long OwnerId { get; set; }
        public long BuyerId { get; set; }
        public long TextureId { get; set; }
        public int Status { get; set; }
    }
} 