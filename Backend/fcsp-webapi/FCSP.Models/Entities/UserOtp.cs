using System;
namespace FCSP.Models.Entities;

public class UserOtp : BaseEntity
{
    public long UserId { get; set; }
    public string OtpCode { get; set; } = null!;
    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; }
    public string PurposeType { get; set; } = null!;

    // Navigation properties
    public virtual User User { get; set; } = null!;
} 