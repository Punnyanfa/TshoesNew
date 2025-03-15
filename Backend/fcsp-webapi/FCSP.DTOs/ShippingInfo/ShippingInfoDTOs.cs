namespace FCSP.DTOs.ShippingInfo
{
    public class GetShippingInfoByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetShippingInfoByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddShippingInfoRequest
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class AddShippingInfoResponse
    {
        public long Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class UpdateShippingInfoRequest
    {
        public long Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class UpdateShippingInfoResponse
    {
        public long Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class DeleteShippingInfoRequest
    {
        public long Id { get; set; }
    }

    public class DeleteShippingInfoResponse
    {
        public bool Success { get; set; }
    }

    public class GetShippingInfosByUserRequest
    {
        public long UserId { get; set; }
    }
} 