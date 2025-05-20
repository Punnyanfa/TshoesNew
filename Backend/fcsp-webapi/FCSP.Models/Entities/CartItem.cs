using System;
using System.Collections.Generic;

namespace FCSP.Models.Entities;

public class CartItem : BaseEntity
{
    public long CartId { get; set; }
    public long CustomShoeDesignId { get; set; }
    public long SizeId { get; set; }
    public long? ManufacturerId { get; set; }
    public int Quantity { get; set; } = 1;
    
    // Navigation properties
    public virtual Cart Cart { get; set; } = null!;
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
    public virtual Size Size { get; set; } = null!;
    public virtual Manufacturer? Manufacturer { get; set; }
} 