using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Service
{
    public class ServiceResponseDto
    {
        public long Id { get; set; }
        public string Component { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Price { get; set; } 
        public long ManufacturerId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GetServiceByIdRequest
    {
        public long Id { get; set; }
    }

    public class AddServiceRequest
    {
        public IEnumerable<AddService> AddServices { get; set; }
    }

    public class AddService
    {
        public string Component { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Price { get; set; } 
        public long ManufacturerId { get; set; }
    }

    public class AddServiceResponse
    {
        public bool Success { get; set; }
    }

    public class UpdateServiceRequest
    {
        public long Id { get; set; }
        public string Component { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Price { get; set; } 
    }

    public class UpdateServiceResponse
    {
        public long Id { get; set; }
        public string Component { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Price { get; set; } 
        public long ManufacturerId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DeleteServiceRequest
    {
        public long Id { get; set; }
    }

    public class DeleteServiceResponse
    {
        public bool Success { get; set; }
    }
}