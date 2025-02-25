namespace FCSP.DTOs.UserTexture
{
    public class GetUserTextureByIdResponse
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long BuyerId { get; set; }
        public long TextureId { get; set; }
        public int Status { get; set; }
        
        // Additional properties to include texture details
        public string? TextureImageUrl { get; set; }
        public int? TexturePrice { get; set; }
        public string? OwnerName { get; set; }
        public string? BuyerName { get; set; }
    }
} 