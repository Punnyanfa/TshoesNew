namespace FCSP.DTOs.Designer
{
    public class GetDesignerRequest
    {
        public long Id { get; set; }
    }

    public class GetDesignerResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Description { get; set; }
        public float CommissionRate { get; set; }
        public float Rating { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddDesignerRequest
    {
        public long UserId { get; set; }
        public string? Description { get; set; }
        public float CommissionRate { get; set; }
        public float Rating { get; set; }
        public int Status { get; set; }
    }

    public class AddDesignerResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateDesignerRequest
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public float CommissionRate { get; set; }
        public int Status { get; set; }
    }

    public class UpdateDesignerResponse
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 