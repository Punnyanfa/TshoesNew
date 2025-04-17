using FCSP.Common.Enums;

namespace FCSP.DTOs.Criteria
{
    public class GetCriteriaRequest
    {
        public long Id { get; set; }
    }

    public class GetCriteriaResponse
    {     

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddCriteriaRequest
    {
        public string Name { get; set; } = null!;
      
    }

    public class AddCriteriaResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateCriteriaRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
       
    }

    public class UpdateCriteriaResponse
    {
       
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; }       
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateCriteriaStatusRequest
    {
        public long Id { get; set; }
       
    }

    public class UpdateCriteriaStatusResponse
    {
        public long Id { get; set; }
        public string  Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 