using System.Collections.Generic;
namespace FCSP.Models.Entities;

public class Service : BaseEntity
{
    public long ManufacturerId { get; set; }
    public string ServiceName { get; set; } = null!;
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual Manufacturer Manufacturer { get; set; } = null!;
    public virtual ICollection<DesignService> DesignServices { get; } = [];
    public virtual ICollection<SetServiceAmount> SetServiceAmounts { get; } = [];
} 