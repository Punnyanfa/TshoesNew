using System.ComponentModel.DataAnnotations.Schema;

namespace FCSP.Models.Entities;

public class DesignService : BaseEntity
{
    // This string property maps to the database column which is nvarchar
    public long CustomShoeDesignId { get; set; }
    
    // This float property matches the database column
    public long ServiceId { get; set; }
    
    // Service price for this design
    public float? Price { get; set; }

    // Navigation properties
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
    public virtual Service Service { get; set; } = null!;
} 