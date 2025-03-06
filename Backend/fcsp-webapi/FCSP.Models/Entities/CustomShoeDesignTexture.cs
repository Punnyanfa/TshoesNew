namespace FCSP.Models.Entities;

public class CustomShoeDesignTexture : BaseEntity
{
    public long CustomShoeDesignId { get; set; }

    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;

    public long TextureId { get; set; }

    public virtual Texture Texture { get; set; } = null!;
}
