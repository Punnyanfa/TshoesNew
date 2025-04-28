namespace FCSP.Models.Entities;

public class OrderDetail : BaseEntity
{
    public long OrderId { get; set; }

    public long ManufacturerId { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public long CustomShoeDesignId { get; set; }

    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;

    public long SizeId { get; set; }

    public virtual Size Size { get; set; } = null!;

    public int Quantity { get; set; }

    public float Price { get; set; }

    // Navigation properties
    public virtual ICollection<Transaction> Transactions { get; } = [];
}
