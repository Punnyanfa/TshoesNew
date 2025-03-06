namespace FCSP.DTOs.Service
{
    public class GetServiceByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetServiceByIdResponse
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Maps to ServiceName in the entity
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Maps to ServiceFee in the entity
        /// </summary>
        public float Price { get; set; }
        
        /// <summary>
        /// Indicates if the service is deleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class AddServiceRequest
    {
        /// <summary>
        /// Maps to ServiceName in the entity
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Maps to ServiceFee in the entity
        /// </summary>
        public float Price { get; set; }
    }

    public class AddServiceResponse
    {
        public long ServiceId { get; set; }
    }

    public class UpdateServiceRequest
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Maps to ServiceName in the entity
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Maps to ServiceFee in the entity
        /// </summary>
        public float Price { get; set; }
    }

    public class UpdateServiceResponse
    {
        public long ServiceId { get; set; }
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