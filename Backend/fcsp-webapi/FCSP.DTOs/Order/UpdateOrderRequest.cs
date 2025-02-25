using FCSP.Common.Enums;

namespace FCSP.DTOs.Order
{
    public class UpdateOrderRequest
    {
        public long Id { get; set; }
        public long? ShippingInfoId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? AmountPaid { get; set; }
        public OrderStatus? Status { get; set; }
        public OrderShippingStatus? ShippingStatus { get; set; }
    }
} 