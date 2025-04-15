using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities;

public class Rating : BaseEntity
{
    public long CustomShoeDesignId { get; set; }
    public long UserId { get; set; }
    public int UserRating { get; set; } 
    public string? Comment { get; set; }

    // Navigation properties
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
    public virtual User User { get; set; } = null!;
} 