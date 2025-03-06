using System.Collections.Generic;
namespace FCSP.Models.Entities;

public class Service : BaseEntity
{
    public string ServiceName { get; set; } = null!;
    public float ServiceFee { get; set; }
    public int IsDeleted { get; set; }

    // Navigation property
    public virtual ICollection<DesignService> DesignServices { get; } = [];
} 