namespace FCSP.Models.Entities;

public class Texture : BaseEntity
{
    public long? UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public virtual ICollection<CustomShoeDesignTexture> CustomShoeDesignTextures { get; } = [];
}
