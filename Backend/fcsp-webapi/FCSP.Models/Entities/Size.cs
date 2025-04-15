namespace FCSP.Models.Entities;

public class Size : BaseEntity
{
    public int SizeValue { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; } = [];
} 