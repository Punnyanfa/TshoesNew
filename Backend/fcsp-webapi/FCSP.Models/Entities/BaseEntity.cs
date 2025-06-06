using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FCSP.Common.Utils;

namespace FCSP.Models.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();

    public DateTime UpdatedAt { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();

    [Timestamp]
    public byte[]? Version { get; } = [];
    
    public bool IsDeleted { get; set; }
}
