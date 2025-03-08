using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class PaymentGateway : BaseEntity
{
    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public PaymentMethod PaymentMethod { get; set; }
    
    public int Status { get; set; }
}
