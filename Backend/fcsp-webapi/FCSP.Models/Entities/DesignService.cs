using System.ComponentModel.DataAnnotations.Schema;

namespace FCSP.Models.Entities;

public class DesignService : BaseEntity
{
    // This string property maps to the database column which is nvarchar
    public long DesignId { get; set; }
    
    // This float property matches the database column
    public long ServiceId { get; set; }

    public virtual CustomShoeDesign? CustomShoeDesign { get; set; }

    public virtual Service? Service { get; set; }
} 