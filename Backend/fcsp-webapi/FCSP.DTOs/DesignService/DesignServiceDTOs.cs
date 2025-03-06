namespace FCSP.DTOs.DesignService
{
    public class GetDesignServiceByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetDesignServiceByIdResponse
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Foreign key to CustomShoeDesign - String type to match database column (nvarchar)
        /// </summary>
        public string DesignId { get; set; } = null!;
        
        /// <summary>
        /// Foreign key to Service - Float type to match database column
        /// </summary>
        public float ServiceId { get; set; }
        
        /// <summary>
        /// Maps to the CustomShoeDesignIdNavigation property in the entity
        /// Used for navigation purposes only - not mapped to database
        /// </summary>
        public long? CustomShoeDesignId { get; set; }
        
        // Navigation property details
        public string? CustomShoeDesignName { get; set; }
        public string? ServiceName { get; set; }
    }

    public class AddDesignServiceRequest
    {
        /// <summary>
        /// Foreign key to CustomShoeDesign - String type to match database column (nvarchar)
        /// </summary>
        public string DesignId { get; set; } = null!;
        
        /// <summary>
        /// Foreign key to Service - Float type to match database column
        /// </summary>
        public float ServiceId { get; set; }
        
        /// <summary>
        /// Maps to the CustomShoeDesignIdNavigation property in the entity
        /// Used for navigation purposes only - not mapped to database
        /// </summary>
        public long? CustomShoeDesignId { get; set; }
    }

    public class AddDesignServiceResponse
    {
        public long DesignServiceId { get; set; }
    }

    public class UpdateDesignServiceRequest
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Foreign key to CustomShoeDesign - String type to match database column (nvarchar)
        /// </summary>
        public string DesignId { get; set; } = null!;
        
        /// <summary>
        /// Foreign key to Service - Float type to match database column
        /// </summary>
        public float ServiceId { get; set; }
        
        /// <summary>
        /// Maps to the CustomShoeDesignIdNavigation property in the entity
        /// Used for navigation purposes only - not mapped to database
        /// </summary>
        public long? CustomShoeDesignId { get; set; }
    }

    public class UpdateDesignServiceResponse
    {
        public long DesignServiceId { get; set; }
    }

    public class DeleteDesignServiceRequest
    {
        public long Id { get; set; }
    }

    public class DeleteDesignServiceResponse
    {
        public bool Success { get; set; }
    }
} 