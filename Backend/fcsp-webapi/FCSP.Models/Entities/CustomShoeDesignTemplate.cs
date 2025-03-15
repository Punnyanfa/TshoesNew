namespace FCSP.Models.Entities;

public class CustomShoeDesignTemplate : BaseEntity
{
    public long? UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Gender { get; set; }
    public string? Color { get; set; }
    public float Price { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("2DImageUrl")]
    public string? TwoDImageUrl { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("3DFileUrl")]
    public string? ThreeDFileUrl { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CustomShoeDesign> CustomShoeDesigns { get; } = [];
}
