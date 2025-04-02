using FCSP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Order
{
    public class GetOrderByIdRequest
    {
        [Required]
        public long Id { get; set; }
    }

    public class GetOrderByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        public float TotalPrice { get; set; } 
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = [];
    }

    public class AddOrderRequest
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        [Required]
        public float TotalPrice { get; set; } 
        [Required]
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = [];
    }

    public class AddOrderResponse
    {
        public long Id { get; set; }
        public float TotalPrice { get; set; } 
        public OrderStatus Status { get; set; } 
    }

    public class UpdateOrderRequest
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        [Required]
        public float TotalPrice { get; set; } 
        [Required]
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>(); // Add this property
    }

    public class UpdateOrderResponse
    {
        public long Id { get; set; }
        public float TotalPrice { get; set; } 
        public OrderStatus Status { get; set; }
    }

    public class DeleteOrderRequest
    {
        [Required]
        public long Id { get; set; }
    }

    public class DeleteOrderResponse
    {
        public bool Success { get; set; }
    }

    public class GetOrdersByUserIdRequest
    {
        [Required]
        public long UserId { get; set; }
    }

    public class GetOrderByUserIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        public float TotalPrice { get; set; } 
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = [];
    }

    public class OrderDetailDto
    {
        [Required]
        public long CustomShoeDesignId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPrice { get; set; } 
        public string? Size { get; set; }
    }
   
}