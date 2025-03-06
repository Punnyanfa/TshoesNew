using System.ComponentModel.DataAnnotations.Schema;

namespace FCSP.Models.Entities;

public class DesignService : BaseEntity
{
    // This string property maps to the database column which is nvarchar
    public string DesignId { get; set; } = null!;
    
    // This float property matches the database column
    public float ServiceId { get; set; }
    
    // Property to allow easy fetching of the related CustomShoeDesign
    [NotMapped]
    public long? CustomShoeDesignIdNavigation { get; set; }

    // Navigation properties
    [ForeignKey("DesignId")]
    public virtual CustomShoeDesign? CustomShoeDesign { get; set; }
    
    [ForeignKey("ServiceId")]
    public virtual Service? Service { get; set; }
} 