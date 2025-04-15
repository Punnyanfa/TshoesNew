using FCSP.DTOs;
using FCSP.Common.Enums;

namespace FCSP.DTOs.Payment
{
    public class PaymentListResponse
    {
        public List<GetPaymentByIdResponse> Payments { get; set; }
    }

    public class GetPaymentByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetPaymentByIdResponse
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public float Amount { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public class AddPaymentRequest
    {
        public long OrderId { get; set; }
        public int Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class AddPaymentResponse
    {
        public string Response { get; set; } = string.Empty;
    }

    public class UpdatePaymentRequest
    {
        public long Id { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public class UpdatePaymentResponse
    {
        public bool Success { get; set; }
    }
}