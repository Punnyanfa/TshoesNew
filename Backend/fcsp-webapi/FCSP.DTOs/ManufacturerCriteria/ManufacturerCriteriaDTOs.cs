using FCSP.Common.Enums;

namespace FCSP.DTOs.ManufacturerCriteria
{
    public class GetManufacturerCriteriaRequest
    {
        public long Id { get; set; }
    }

    public class GetManufacturerCriteriaResponse
    {
        public long Id { get; set; }
        public long ManufacturerId { get; set; }     
        public long CriteriaId { get; set; }
        public string Status { get; set; }
        public string ManufacturerName { get; set; } = null!;
        public string CriteriaName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddManufacturerCriteriaRequest
    {
        public long ManufacturerId { get; set; }
        public long CriteriaId { get; set; }
        public ManufacturerCriteriaStatus Status { get; set; }
    }

    public class AddManufacturerCriteriaResponse
    {
        public long Id { get; set; }
        public long ManufacturerId { get; set; }
        public long CriteriaId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateManufacturerCriteriaRequest
    {
        public long Id { get; set; }
        public ManufacturerCriteriaStatus Status { get; set; }
    }

    public class UpdateManufacturerCriteriaResponse
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}