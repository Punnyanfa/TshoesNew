using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Service
{
    public class GetServiceByIdRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Service ID must be greater than 0")]
        public long Id { get; set; }
    }

    public class GetServiceByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AddServiceRequest
    {
        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Service name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [Range(0, float.MaxValue, ErrorMessage = "Price cannot be negative")]
        public float Price { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Manufacturer ID must be greater than 0")]
        public long ManufacturerId { get; set; }
    }

    public class AddServiceResponse
    {
        public long ServiceId { get; set; }
    }

    public class UpdateServiceRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Service ID must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Service name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [Range(0, float.MaxValue, ErrorMessage = "Price cannot be negative")]
        public float Price { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Manufacturer ID must be greater than 0")]
        public long ManufacturerId { get; set; }
    }

    public class UpdateServiceResponse
    {
        public long ServiceId { get; set; }
    }

    public class DeleteServiceRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "Service ID must be greater than 0")]
        public long Id { get; set; }
    }

    public class DeleteServiceResponse
    {
        public bool Success { get; set; }
    }
}