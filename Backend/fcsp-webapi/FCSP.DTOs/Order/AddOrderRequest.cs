using FCSP.Common.Enums;
using FCSP.DTOs;

namespace FCSP.DTOs.Order
{
    public class AddOrderRequest
    {
        public long? UserId { get; set; }
        public long? ShippingInfoId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? AmountPaid { get; set; }
        public OrderStatus Status { get; set; }
        public OrderShippingStatus? ShippingStatus { get; set; }
        public List<AddOrderDetailDto> OrderDetails { get; set; } = new List<AddOrderDetailDto>();
    }

    public class AddOrderDetailDto
    {
        public long? CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
} 