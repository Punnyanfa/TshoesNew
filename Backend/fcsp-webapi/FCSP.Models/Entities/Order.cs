using FCSP.Common.Enums;

namespace FCSP.Models.Entities;

public class Order : BaseEntity
{
    public long? UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public long? ShippingInfoId { get; set; }

    public virtual ShippingInfo ShippingInfo { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public decimal? AmountPaid { get; set; }

    public OrderStatus Status { get; set; }

    public OrderShippingStatus? ShippingStatus { get; set; }

    public virtual IEnumerable<OrderDetail> OrderDetails { get; } = [];

    public virtual IEnumerable<Payment> Payments { get; } = [];
}
