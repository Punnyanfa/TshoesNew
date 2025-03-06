using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities;

public class ShippingInfo : BaseEntity
{
    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string District { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    public int IsDeleted { get; set; }
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; } = [];
}
