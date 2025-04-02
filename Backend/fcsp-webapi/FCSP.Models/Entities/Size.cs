namespace FCSP.Models.Entities;

public class Size : BaseEntity
{
    public int SizeValue { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual ICollection<CustomShoeDesign> CustomShoeDesigns { get; } = [];
    public virtual ICollection<OrderDetail> OrderDetails { get; } = [];
} 