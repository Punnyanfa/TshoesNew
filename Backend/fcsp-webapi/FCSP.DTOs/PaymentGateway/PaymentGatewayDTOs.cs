using FCSP.DTOs;

namespace FCSP.DTOs.PaymentGateway
{
    public class PaymentGatewayDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaymentGatewayListResponse : BaseResponseModel<List<PaymentGatewayDto>>
    {
    }

    public class PaymentGatewayResponse : BaseResponseModel<PaymentGatewayDto>
    {
    }

    public class GetPaymentGatewayByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetPaymentGatewayByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddPaymentGatewayRequest
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class AddPaymentGatewayResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class UpdatePaymentGatewayRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class UpdatePaymentGatewayResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class DeletePaymentGatewayRequest
    {
        public long Id { get; set; }
    }

    public class DeletePaymentGatewayResponse
    {
        public bool Success { get; set; }
    }

    public class GetPaymentGatewaysByUserRequest
    {
        public long UserId { get; set; }
    }
} 