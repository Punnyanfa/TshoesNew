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
        public int Amount { get; set; }
        public PaymentStatus Status { get; set; }
    }


    public class GetPaymentInfoRequest  
    {
        public long PaymentId { get; set; }
    }

    public class GetPaymentInfoResponse
    {
        public long PaymentId { get; set; }
        public String Status { get; set; } = string.Empty;
        public int Amount { get; set; }
        public int AmountPaid { get; set; }
        public int AmountRemaining { get; set; }
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
        public String Status { get; set; } = string.Empty;
    }

    public class UpdatePaymentResponse
    {
        public bool Success { get; set; }
    }

    public class CancelPaymentRequest
    {
        public long PaymentId { get; set; }
    }

    public class CancelPaymentResponse
    {
        public bool Success { get; set; }
    }
    
    public class ConfirmWebhookRequest
    {
        public string WebhookUrl { get; set; }
    }

    public class ConfirmWebhookResponse
    {
        public bool Success { get; set; }
    }

    public class UpdatePaymentUsingWebhookRequest
    {
        public long Id { get; set; }
        public String Status { get; set; } = string.Empty;
    }

    public class UpdatePaymentUsingWebhookResponse
    {
        public bool Success { get; set; }
    }
}