namespace FCSP.Models.Entities;

public class Texture : BaseEntity
{
    public long? UserId { get; set; }
    public int? Price { get; set; }

    public virtual User User { get; set; } = null!;

    public string? ImageUrl { get; set; }
    public string? Prompt { get; set; }

    public virtual ICollection<CustomShoeDesignTexture> CustomShoeDesignTextures { get; } = [];
    
    // New navigation property
    public virtual ICollection<UserTexture> UserTextures { get; } = [];
}
