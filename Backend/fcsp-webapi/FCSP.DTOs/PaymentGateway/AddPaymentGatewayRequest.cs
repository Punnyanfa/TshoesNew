using FCSP.Common.Enums;

namespace FCSP.DTOs.PaymentGateway
{
    public class AddPaymentGatewayRequest
    {
        public long? UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
} 