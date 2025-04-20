using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class CustomShoeDesignTemplate : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Gender { get; set; }
    public string? Color { get; set; }
    public float Price { get; set; }
    public TemplateStatus Status { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("2DImageUrl")]
    public string? TwoDImageUrl { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("3DFileUrl")]
    public string? ThreeDFileUrl { get; set; }
    public virtual ICollection<CustomShoeDesign> CustomShoeDesigns { get; } = [];
}
