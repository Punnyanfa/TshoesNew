using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Service
{
    public class ServiceResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; } // Thay đổi từ decimal sang float
        public long ManufacturerId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GetServiceByIdRequest
    {
        [Required(ErrorMessage = "ServiceId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public long Id { get; set; }
    }

    public class AddServiceRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Price must be greater than 0.")] // Thay đổi từ double sang float
        public float Price { get; set; } // Thay đổi từ decimal sang float

        [Required(ErrorMessage = "ManufacturerId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ManufacturerId must be greater than 0.")]
        public long ManufacturerId { get; set; }
    }

    public class AddServiceResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; } // Thay đổi từ decimal sang float
        public long ManufacturerId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UpdateServiceRequest
    {
        [Required(ErrorMessage = "ServiceId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Price must be greater than 0.")] // Thay đổi từ double sang float
        public float Price { get; set; } // Thay đổi từ decimal sang float
    }

    public class UpdateServiceResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; } // Thay đổi từ decimal sang float
        public long ManufacturerId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DeleteServiceRequest
    {
        [Required(ErrorMessage = "ServiceId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public long Id { get; set; }
    }

    public class DeleteServiceResponse
    {
        public bool Success { get; set; }
    }
}