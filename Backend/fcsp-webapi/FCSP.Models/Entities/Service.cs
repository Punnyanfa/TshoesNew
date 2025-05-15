using System.Collections.Generic;
namespace FCSP.Models.Entities;

public class Service : BaseEntity
{
    public long ManufacturerId { get; set; }
    public string Component { get; set; } = null!;
    public string Type { get; set; } = null!;
    
    public int Price { get; set; }

    // Navigation properties
    public virtual Manufacturer Manufacturer { get; set; } = null!;
    public virtual ICollection<DesignService> DesignServices { get; } = [];
} 