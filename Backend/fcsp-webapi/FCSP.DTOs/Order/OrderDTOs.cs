using FCSP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Order
{
    public class GetOrderByIdRequest
    {
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public long Id { get; set; }
    }

    public class GetOrderByIdResponse
    {        
        public long Id { get; set; }       
        public string UserName { get; set; }
        public long ShippingInfoId { get; set; }       
        public string VoucherCode { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
       public string ShippingStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailResponseDto> OrderDetails { get; set; } = [];
    }

    public class AddOrderRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "ShippingInfoId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ShippingInfoId must be greater than 0.")]
        public long ShippingInfoId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "VoucherId must be greater than 0 if provided.")]
        public long? VoucherId { get; set; }

        [Required(ErrorMessage = "PaymentMethod is required.")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "OrderDetails are required.")]
        [MinLength(1, ErrorMessage = "Order must contain at least one OrderDetail.")]
        public List<OrderDetailRequestDto> OrderDetails { get; set; } = [];
    }

    public class AddOrderResponse
    {
        public string? PaymentUrl { get; set; }
    }

    public class UpdateOrderRequest
    {
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public OrderStatus Status { get; set; }
    }

    public class UpdateOrderResponse
    {
        public bool Success { get; set; }
    }

    public class CancelOrderRequest
    {
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public long Id { get; set; }
    }

    public class CancelOrderResponse
    {
        public bool Success { get; set; }
    }

    public class GetOrdersByUserIdRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public long UserId { get; set; }
    }

    // DTO cho request (không có UnitPrice)
    public class OrderDetailRequestDto
    {
        [Required(ErrorMessage = "CustomShoeDesignId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "CustomShoeDesignId must be greater than 0.")]
        public long CustomShoeDesignId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "SizeId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "SizeId must be greater than 0.")]
        public long SizeId { get; set; }
    }

    // DTO cho response (có UnitPrice)
    public class OrderDetailResponseDto
    {
        public long CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public long SizeValue { get; set; }
    }

    public class ProcessPaymentRequest
    {
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public long OrderId { get; set; }
    }

    public class ProcessPaymentResponse
    {
        public long PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public long OrderId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "ShippingStatus is required.")]
        public OrderShippingStatus ShippingStatus { get; set; }
    }

    public class UpdateOrderStatusResponse
    {
        public long OrderId { get; set; }
        public string Status { get; set; }
        public string ShippingStatus { get; set; }
      
    }
}