namespace FCSP.Models.Entities;

public class CustomShoeDesignTexture : BaseEntity
{

    public long? CustomShoeDesignId { get; set; }

    public CustomShoeDesign CustomShoeDesign { get; set; } = null!;

    public long? TextureId { get; set; }

    public Texture Texture { get; set; } = null!;
}
