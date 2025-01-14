using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities;

public class ShippingInfo : BaseEntity
{
    public long? UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public string? StreetAddress { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}
