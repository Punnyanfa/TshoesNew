using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Service
{
    public class ServiceResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public int Price { get; set; } 
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

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Component is required.")]
        [StringLength(100, ErrorMessage = "Component cannot be longer than 100 characters.")]
        public string Component { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")] 
        public int Price { get; set; } 

        [Required(ErrorMessage = "ManufacturerId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ManufacturerId must be greater than 0.")]
        public long ManufacturerId { get; set; }
    }

    public class AddServiceResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public int Price { get; set; } 
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

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Component is required.")]
        [StringLength(100, ErrorMessage = "Component cannot be longer than 100 characters.")]
        public string Component { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")] 
        public int Price { get; set; } 
    }

    public class UpdateServiceResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public int Price { get; set; } 
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