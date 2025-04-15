using System;
using System.Collections.Generic;

namespace FCSP.Models.Entities;

public class Cart : BaseEntity
{
    public long UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; } = [];
} 