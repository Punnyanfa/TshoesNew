namespace FCSP.DTOs.DesignService
{
    public class GetDesignServiceByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetDesignServiceByIdResponse
    {
        public long Id { get; set; }
        public long DesignId { get; set; }
        public long ServiceId { get; set; }
        public string? CustomShoeDesignName { get; set; }
        public string? ServiceName { get; set; }
    }

    public class AddDesignServiceRequest
    {
        public long DesignId { get; set; }
        
        public long ServiceId { get; set; }
    }

    public class AddDesignServiceResponse
    {
        public long DesignServiceId { get; set; }
    }

    public class UpdateDesignServiceRequest
    {
        public long Id { get; set; }

        public long DesignId { get; set; }

        public long ServiceId { get; set; }
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