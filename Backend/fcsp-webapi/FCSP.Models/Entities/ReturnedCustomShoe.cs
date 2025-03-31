namespace FCSP.Models.Entities;

public class ReturnedCustomShoe : BaseEntity
{
    public long CustomShoeDesignId { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
} 