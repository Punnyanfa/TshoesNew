using FCSP.DTOs;

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
        public float? Price { get; set; } // Giữ lại để trả về giá đã tính
        public string? CustomShoeDesignName { get; set; }
        public string? ServiceName { get; set; }
    }

    public class AddDesignServiceRequest
    {
        public long DesignId { get; set; }
        public long ServiceId { get; set; }
        // Bỏ thuộc tính Price vì sẽ tính tự động
    }

    public class AddDesignServiceResponse
    {
        public long DesignServiceId { get; set; }
        public float? Price { get; set; } // Thêm để trả về giá đã tính
    }

    public class UpdateDesignServiceRequest
    {
        public long Id { get; set; }
        public long DesignId { get; set; }
        public long ServiceId { get; set; }
        // Bỏ thuộc tính Price vì sẽ tính tự động
    }

    public class UpdateDesignServiceResponse
    {
        public long DesignServiceId { get; set; }
        public float? Price { get; set; } // Thêm để trả về giá đã tính
    }

    public class DeleteDesignServiceRequest
    {
        public long Id { get; set; }
    }

    public class DeleteDesignServiceResponse
    {
        public bool Success { get; set; }
    }

    public class DesignServiceDto
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public long ServiceId { get; set; }
        public float? Price { get; set; } // Giữ lại để trả về giá đã tính
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetDesignServicesByCustomShoeDesignIdRequest
    {
        public long CustomShoeDesignId { get; set; }
    }

    public class GetDesignServicesByServiceIdRequest
    {
        public long ServiceId { get; set; }
    }
}