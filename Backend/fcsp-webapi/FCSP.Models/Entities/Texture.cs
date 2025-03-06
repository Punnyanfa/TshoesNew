namespace FCSP.Models.Entities;

public class Texture : BaseEntity
{
    public long UserId { get; set; }
    public string? Prompt { get; set; }
    public int Status { get; set; }
    public int IsDeleted { get; set; }
    public string ImageUrl { get; set; } = null!;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CustomShoeDesignTexture> CustomShoeDesignTextures { get; } = [];
}
