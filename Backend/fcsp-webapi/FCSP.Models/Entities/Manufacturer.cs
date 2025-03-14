using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Manufacturer : BaseEntity
{
    public long UserId { get; set; }
    public string Name { get; set; } = null!;
    public float CommissionRate { get; set; }
    public ManufacturerStatus Status { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Service> Services { get; } = [];
    public virtual ICollection<ManufacturerCriteria> ManufacturerCriterias { get; } = [];
} 