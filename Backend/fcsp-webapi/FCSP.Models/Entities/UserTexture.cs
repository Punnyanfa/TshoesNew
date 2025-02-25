namespace FCSP.Models.Entities;

public class UserTexture : BaseEntity
{
    public long OwnerId { get; set; }
    
    public long BuyerId { get; set; }
    
    public long TextureId { get; set; }
    
    public int Status { get; set; }
    
    // Navigation properties
    public virtual User Owner { get; set; } = null!;
    
    public virtual User Buyer { get; set; } = null!;
    
    public virtual Texture Texture { get; set; } = null!;
} 