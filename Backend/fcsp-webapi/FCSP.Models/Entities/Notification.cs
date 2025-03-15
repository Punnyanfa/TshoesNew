using System.ComponentModel.DataAnnotations;
namespace FCSP.Models.Entities;
public class Notification : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsDeleted { get; set; }

    // Navigation property
    public virtual User User { get; set; } = null!;
} 