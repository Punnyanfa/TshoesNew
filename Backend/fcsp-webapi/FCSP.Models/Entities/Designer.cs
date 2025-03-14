using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Designer : BaseEntity
{
    public long UserId { get; set; }
    public string? Description { get; set; }
    public float CommissionRate { get; set; }
    public float Rating { get; set; }
    public DesignerStatus Status { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
} 