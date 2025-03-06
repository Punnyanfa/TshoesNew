using System;
using System.Collections.Generic;
namespace FCSP.Models.Entities;
public class Voucher : BaseEntity
{
    public string? VoucherName { get; set; }
    public string? VoucherValue { get; set; }
    public string? Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Status { get; set; }

    // Navigation property
    public virtual ICollection<Order> Orders { get; } = [];
} 