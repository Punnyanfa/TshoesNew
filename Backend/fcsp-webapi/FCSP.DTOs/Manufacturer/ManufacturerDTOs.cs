namespace FCSP.DTOs.Manufacturer
{
    public class GetManufacturerRequest
    {
        public long Id { get; set; }
    }

    public class GetManufacturerResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public float CommissionRate { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetManufacturerDetailResponse : GetManufacturerResponse
    {
        public IEnumerable<ServiceDto>? Services { get; set; }
        public IEnumerable<CriteriaDto>? Criterias { get; set; }
    }

    public class ServiceDto
    {
        public long Id { get; set; }
        public string ServiceName { get; set; } = null!;
        public float? CurrentAmount { get; set; }
    }

    public class CriteriaDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AddManufacturerRequest
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public float CommissionRate { get; set; }
        public int Status { get; set; }
    }

    public class AddManufacturerResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateManufacturerRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public float CommissionRate { get; set; }
        public int Status { get; set; }
    }

    public class UpdateManufacturerResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 