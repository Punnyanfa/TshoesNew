namespace FCSP.Models.Entities;

public class Size : BaseEntity
{
    public string Value { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; } = [];
} 