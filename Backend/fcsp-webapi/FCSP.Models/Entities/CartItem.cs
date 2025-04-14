using System;
using System.Collections.Generic;

namespace FCSP.Models.Entities;

public class CartItem : BaseEntity
{
    public long CartId { get; set; }
    public long CustomShoeDesignId { get; set; }
    public int Quantity { get; set; } = 1;
    public float Price { get; set; }
    
    // Navigation properties
    public virtual Cart Cart { get; set; } = null!;
    public virtual CustomShoeDesign CustomShoeDesign { get; set; } = null!;
} 