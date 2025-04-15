using FCSP.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.PaymentGateway
{
    public class PaymentGatewayDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string AccountNumber { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaymentGatewayResponse : BaseResponseModel<PaymentGatewayDto>
    {
    }

    public class PaymentGatewayListResponse : BaseResponseModel<List<PaymentGatewayDto>>
    {
    }

    public class GetPaymentGatewayByIdRequest
    {
        [Required]
        public long Id { get; set; }
    }

    public class GetPaymentGatewaysByUserRequest
    {
        [Required]
        public long UserId { get; set; }
    }

    public class AddPaymentGatewayRequest
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Provider { get; set; }
    }

    public class UpdatePaymentGatewayRequest
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Provider { get; set; }
    }

    public class DeletePaymentGatewayRequest
    {
        [Required]
        public long Id { get; set; }
    }
} 