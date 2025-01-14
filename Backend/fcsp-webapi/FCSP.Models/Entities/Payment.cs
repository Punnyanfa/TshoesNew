using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Payment : BaseEntity
{
    public long? OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
}
