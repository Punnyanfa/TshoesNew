namespace FCSP.Models.Entities;

public class CustomShoeDesignImage : BaseEntity
{
    public long CustomShoeDesignId { get; set; }
    public long TextureId { get; set; }

    // Navigation properties
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
    public virtual Texture Texture { get; set; } = null!;
} 