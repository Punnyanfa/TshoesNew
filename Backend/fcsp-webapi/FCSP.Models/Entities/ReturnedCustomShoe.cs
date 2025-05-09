namespace FCSP.Models.Entities;

public class ReturnedCustomShoe : BaseEntity
{
    public long CustomShoeDesignId { get; set; }
    public int Price { get; set; }
    public int Size { get; set; }
    public int Quantity { get; set; }

    // Navigation properties
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
} 