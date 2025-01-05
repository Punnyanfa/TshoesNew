namespace FCSP.Models.Entities;

public class OrderDetail : BaseEntity
{
    public long? OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public long? CustomShoeDesignId { get; set; }

    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal? Price { get; set; }
}
