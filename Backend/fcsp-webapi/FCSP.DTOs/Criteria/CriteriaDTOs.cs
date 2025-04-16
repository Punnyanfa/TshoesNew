using FCSP.Common.Enums;

namespace FCSP.DTOs.Criteria
{
    public class GetCriteriaRequest
    {
        public long Id { get; set; }
    }

    public class GetCriteriaResponse
    {
        private readonly CriteriaStatus status;
        public GetCriteriaResponse(CriteriaStatus status)
        {
            this.status = status;
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
    
        public string StatusName => EnumHelper.GetEnumName<CriteriaStatus>((int)status);
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
        public CriteriaStatus Status { get; set; }
    }

    public class UpdateCriteriaResponse
    {
        private readonly CriteriaStatus status;
        public UpdateCriteriaResponse(CriteriaStatus status)
        {
            this.status = status;
        }
        public long Id { get; set; }
        public string Name { get; set; } = null!;
       
        public string StatusName => EnumHelper.GetEnumName<CriteriaStatus>((int)status);
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateCriteriaStatusRequest
    {
        public long Id { get; set; }
        public CriteriaStatus Status { get; set; }
    }

    public class UpdateCriteriaStatusResponse
    {
        public long Id { get; set; }
        public CriteriaStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 