namespace FCSP.Models.Entities;

public class CustomShoeDesign : BaseEntity
{
    public long? UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public long? CustomShoeDesignTemplateId { get; set; }

    public virtual CustomShoeDesignTemplate CustomShoeDesignTemplate { get; set; } = null!;

    public string? DesignData { get; set; }

    public string? Preview3DFileUrl { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<CustomShoeDesignTexture> CustomShoeDesignTextures { get; } = [];
}
