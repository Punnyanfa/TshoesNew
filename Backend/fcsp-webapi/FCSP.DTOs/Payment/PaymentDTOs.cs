using FCSP.DTOs;

namespace FCSP.DTOs.Payment
{
    public class PaymentDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaymentListResponse : BaseResponseModel<List<PaymentDto>>
    {
    }

    public class PaymentResponse : BaseResponseModel<PaymentDto>
    {
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
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddPaymentRequest
    {
        public long OrderId { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class AddPaymentResponse
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class UpdatePaymentRequest
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class UpdatePaymentResponse
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class DeletePaymentRequest
    {
        public long Id { get; set; }
    }

    public class DeletePaymentResponse
    {
        public bool Success { get; set; }
    }

    public class GetPaymentsByOrderRequest
    {
        public long OrderId { get; set; }
    }
} 