using System.Collections.Generic;
namespace FCSP.Models.Entities;

public class Service : BaseEntity
{
    public long ManufacturerId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Component { get; set; } = null!;

    // Navigation properties
    public virtual Manufacturer Manufacturer { get; set; } = null!;
    public virtual ICollection<DesignService> DesignServices { get; } = [];
    public virtual ICollection<SetServiceAmount> SetServiceAmounts { get; } = [];
} 