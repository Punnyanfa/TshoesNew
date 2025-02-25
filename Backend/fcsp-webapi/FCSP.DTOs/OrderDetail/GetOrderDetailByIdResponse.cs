namespace FCSP.DTOs.OrderDetail
{
    public class GetOrderDetailByIdResponse
    {
        public long? OrderId { get; set; }
        public long? CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
} 