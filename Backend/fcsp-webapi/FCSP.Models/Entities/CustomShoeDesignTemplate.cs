namespace FCSP.Models.Entities;

public class CustomShoeDesignTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<CustomShoeDesign> CustomShoeDesigns { get; } = [];
}
