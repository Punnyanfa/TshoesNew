using FCSP.Common.Enums;
using FCSP.DTOs.OrderDetail;

namespace FCSP.DTOs.Order
{
    public class GetOrderByIdResponse : BaseResponse
    {
        public long? UserId { get; set; }
        public long? ShippingInfoId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? AmountPaid { get; set; }
        public OrderStatus Status { get; set; }
        public OrderShippingStatus? ShippingStatus { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
} 