using FCSP.Common.Enums;

namespace FCSP.DTOs.PaymentGateway
{
    public class GetPaymentGatewayByIdResponse
    {
        public long? UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
} 