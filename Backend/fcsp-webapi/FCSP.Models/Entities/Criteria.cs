using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Criteria : BaseEntity
{
    public string Name { get; set; } = null!;
    public CriteriaStatus Status { get; set; }

    // Navigation properties
    public virtual ICollection<ManufacturerCriteria> ManufacturerCriterias { get; } = [];
} 