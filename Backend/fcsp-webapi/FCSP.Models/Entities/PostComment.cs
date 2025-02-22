namespace FCSP.Models.Entities
{
    public class PostComment : BaseEntity
    {
        public long? UserId { get; set; }
        public long? PostId { get; set; }
        public string? Comment { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}
