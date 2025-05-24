using FCSP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Manufacturer
{
    public class GetManufacturerRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Manufacturer ID must be greater than 0")]
        public long Id { get; set; }
    }

    public class GetManufacturersByUserRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public long UserId { get; set; }
    }

    public class GetManufacturerDetailResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; } = null!;
        public float CommissionRate { get; set; }
        public string Status { get; set; }
        public List<ServiceDto> Services { get; set; }
        public List<CriteriaDto> Criterias { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ServiceDto
    {
        public long Id { get; set; }
        public string Component { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? CurrentAmount { get; set; }
    }

    public class CriteriaDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AddManufacturerRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 100 characters")]
        public string Description { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Commission rate must be between 0 and 100")]
        public float CommissionRate { get; set; }

        [Range(0, 3, ErrorMessage = "Status must be 0 (Inactive), 1 (Active), 2 (Suspended), or 3 (Pending)")]
        public int Status { get; set; }
    }

    public class AddManufacturerResponse
    {
        public long Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateManufacturerRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Manufacturer ID must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 100 characters")]
        public string Description { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Commission rate must be between 0 and 100")]
        public float CommissionRate { get; set; }

        [Range(0, 3, ErrorMessage = "Status must be 0 (Inactive), 1 (Active), 2 (Suspended), or 3 (Pending)")]
        public int Status { get; set; }
    }

    public class UpdateManufacturerResponse
    {
        public long Id { get; set; }
        public string Description { get; set; } = null!;
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateManufacturerStatusRequest 
    {
        public long Id { get; set; }
        public int Status { get; set; }
    }
    public class UpdateManufacturerStatusResponse
    {
       public bool Success { get; set; }
    }

    public class DeleteManufacturerResponse
    {
        public bool Success { get; set; }
    }
}