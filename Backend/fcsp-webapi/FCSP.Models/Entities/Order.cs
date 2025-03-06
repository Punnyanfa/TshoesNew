using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Order : BaseEntity
{
    public long UserId { get; set; }
    public long? VoucherId { get; set; }
    public long ShippingInfoId { get; set; }
    public float TotalPrice { get; set; }
    public float AmountPaid { get; set; }
    public int Status { get; set; }
    public int ShippingStatus { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Voucher? Voucher { get; set; }
    public virtual ShippingInfo ShippingInfo { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; } = [];
    public virtual ICollection<Payment> Payments { get; } = [];
}
