using FCSP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole UserRole { get; set; } = UserRole.Customer;

    public long? DefaultAddressId { get; set; }

    public ShippingInfo? DefaultAddress { get; set; }

    public virtual ICollection<CustomShoeDesignTemplate> ShoeDisplays { get; } = [];

    public virtual ICollection<ShippingInfo> ShippingInfos { get; } = [];

    public virtual ICollection<PaymentGateway> PaymentGateways { get; } = [];
    
    // New navigation properties for UserTexture
    public virtual ICollection<UserTexture> OwnedTextures { get; } = [];
    public virtual ICollection<UserTexture> PurchasedTextures { get; } = [];
}
