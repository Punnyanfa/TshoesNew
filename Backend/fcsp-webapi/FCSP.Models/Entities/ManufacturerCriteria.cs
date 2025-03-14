using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class ManufacturerCriteria : BaseEntity
{
    public long ManufacturerId { get; set; }
    public long CriteriaId { get; set; }
    public ManufacturerCriteriaStatus Status { get; set; }

    // Navigation properties
    public virtual Manufacturer Manufacturer { get; set; } = null!;
    public virtual Criteria Criteria { get; set; } = null!;
} 