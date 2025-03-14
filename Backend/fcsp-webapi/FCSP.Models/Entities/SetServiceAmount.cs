using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class SetServiceAmount : BaseEntity
{
    public long ServiceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float Amount { get; set; }
    public ServiceAmountStatus Status { get; set; }

    // Navigation properties
    public virtual Service Service { get; set; } = null!;
} 