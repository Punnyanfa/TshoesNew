using FCSP.Common.Enums;

namespace FCSP.DTOs.PaymentGateway
{
    public class GetPaymentGatewayByIdResponse : BaseResponse
    {
        public long? UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
} 