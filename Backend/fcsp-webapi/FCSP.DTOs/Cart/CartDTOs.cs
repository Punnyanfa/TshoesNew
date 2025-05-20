using System.Collections.Generic;

namespace FCSP.DTOs.Cart
{
    public class AddToCartRequest
    {
        public long UserId { get; set; }
        public long CustomShoeDesignId { get; set; }
        public long SizeId { get; set; }
        public long? ManufacturerId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class AddToCartResponse
    {
        public bool Success { get; set; }
        public long CartId { get; set; }
        public long CartItemId { get; set; }
        public int TotalItems { get; set; }
    }

    public class GetCartResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public int TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }

    public class CartItemDto
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public string CustomShoeDesignName { get; set; }
        public long SizeId { get; set; }
        public string SizeValue { get; set; }
        public long? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Subtotal => Price * Quantity;
    }

    public class RemoveFromCartRequest
    {
        public long UserId { get; set; }
        public long CartItemId { get; set; }
    }

    public class RemoveFromCartResponse
    {
        public bool Success { get; set; }
        public int RemainingItems { get; set; }
        public int CartTotal { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public long UserId { get; set; }
        public long CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemResponse
    {
        public bool Success { get; set; }
        public long CartItemId { get; set; }
        public int Quantity { get; set; }
        public int Subtotal { get; set; }
        public int CartTotal { get; set; }
    }
} 