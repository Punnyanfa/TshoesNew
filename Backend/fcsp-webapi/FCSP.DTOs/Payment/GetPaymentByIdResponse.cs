using FCSP.Common.Enums;

namespace FCSP.DTOs.Payment
{
    public class GetPaymentByIdResponse
    {
        public long? OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
} 