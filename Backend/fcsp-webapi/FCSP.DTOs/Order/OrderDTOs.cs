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

    public class GetOrdersByUserIdRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public long UserId { get; set; }
    }

    public class GetOrdersByUserIdResponse
    {
        public IEnumerable<GetOrderByIdResponse> Orders { get; set; } = [];
    }

    public class GetOrderByIdResponse
    {        
        public long Id { get; set; }       
        public string UserName { get; set; }
        public long ShippingInfoId { get; set; }       
        public string VoucherCode { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public string ShippingStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderDetailResponseDto OrderDetail { get; set; } = null!;
    }

    public class GetOrderByManufacturerIdRequest
    {
        [Required(ErrorMessage = "ManufacturerId is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "ManufacturerId must be greater than 0.")]
        public long ManufacturerId { get; set; }
    }

    public class GetOrderByManufacturerIdResponse
    {
        public IEnumerable<GetOrderByIdResponse> Orders { get; set; } = [];
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
        public OrderDetailRequestDto OrderDetail { get; set; } = new OrderDetailRequestDto();
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
        public long ManufacturerId { get; set; }
    }

    // DTO cho response (có UnitPrice)
    public class OrderDetailResponseDto
    {
        public long CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TemplatePrice { get; set; }
        public int ServicePrice { get; set; }
        public int DesignerMarkup { get; set; }
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
}