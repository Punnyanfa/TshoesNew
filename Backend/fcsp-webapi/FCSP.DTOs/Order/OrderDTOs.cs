using FCSP.Common.Enums;

namespace FCSP.DTOs.Order
{
    public class GetOrderByIdRequest
    {
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
        public long UserId { get; set; }
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = [];
    }

    public class AddOrderResponse
    {
        public long Id { get; set; }
        public float TotalPrice { get; set; }
        public int Status { get; set; }
    }

    public class UpdateOrderRequest
    {
        public long Id { get; set; }
        public long ShippingInfoId { get; set; }
        public long? VoucherId { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateOrderResponse
    {
        public long Id { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class DeleteOrderRequest
    {
        public long Id { get; set; }
    }

    public class DeleteOrderResponse
    {
        public bool Success { get; set; }
    }

    public class GetOrdersByUserRequest
    {
        public long UserId { get; set; }
    }

    public class OrderDetailDto
    {
        public long CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public string? Size { get; set; }
    }
} 