using FCSP.Common.Enums;

namespace FCSP.DTOs.PaymentGateway
{
    public class UpdatePaymentGatewayRequest
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
} 