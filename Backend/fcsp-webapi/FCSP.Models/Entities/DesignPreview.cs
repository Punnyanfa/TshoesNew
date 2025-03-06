using System.ComponentModel.DataAnnotations;
namespace FCSP.Models.Entities;

public class DesignPreview : BaseEntity
{
    public long CustomShoeDesignId { get; set; }
    public string? PreviewImageUrl { get; set; }

    // Navigation property
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
} 