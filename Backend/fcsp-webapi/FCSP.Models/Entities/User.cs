using FCSP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Dob { get; set; }
    public string? Gender { get; set; }

    public string? AvatarImageUrl { get; set; }
    public bool IsBanned { get; set; }

    public UserRole UserRole { get; set; }

    public float? Balance { get; set; }

    public long? DefaultAddressId { get; set; }

    // Navigation properties
    public virtual ShippingInfo? DefaultAddress { get; set; }
    public virtual ICollection<CustomShoeDesign> CustomShoeDesigns { get; } = [];
    public virtual ICollection<Order> Orders { get; } = [];
    public virtual ICollection<PaymentGateway> PaymentGateways { get; } = [];
    public virtual ICollection<ShippingInfo> ShippingInfos { get; } = [];
    public virtual ICollection<Posts> Posts { get; } = [];
    public virtual ICollection<PostsComments> PostsComments { get; } = [];
    public virtual ICollection<Notification> Notifications { get; } = [];
    public virtual ICollection<UserActivity> Activities { get; } = [];
    public virtual ICollection<UserRecommendation> Recommendations { get; } = [];
    public virtual ICollection<Rating> Ratings { get; } = [];
    public virtual ICollection<Texture> Textures { get; } = [];
    public virtual ICollection<Transaction> ReceivedTransactions { get; } = [];
    public virtual ICollection<Manufacturer> Manufacturers { get; } = [];
    public virtual ICollection<Designer> Designers { get; } = [];
    public virtual Cart? Cart { get; set; }
}
