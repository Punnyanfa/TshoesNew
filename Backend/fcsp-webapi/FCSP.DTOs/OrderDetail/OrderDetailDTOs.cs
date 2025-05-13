namespace FCSP.DTOs.OrderDetail
{
    public class GetOrderDetailByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetOrderDetailByIdResponse
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long CustomShoeDesignId { get; set; }
        public string CustomShoeDesignName { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public long SizeId { get; set; }
        public long? ManufacturerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddOrderDetailRequest
    {
        public long OrderId { get; set; }
        public long CustomShoeDesignId { get; set; }
        public int Quantity { get; set; }
        public long SizeId { get; set; }
        public long? ManufacturerId { get; set; }
    }

    public class AddOrderDetailResponse
    {
        public bool Success { get; set; }
    }

    public class UpdateOrderDetailRequest
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public long SizeId { get; set; }
        public long? ManufacturerId { get; set; }
    }

    public class UpdateOrderDetailResponse
    {
        public long Id { get; set; }
        public float UnitPrice { get; set; }
    }

    public class DeleteOrderDetailRequest
    {
        public long Id { get; set; }
    }

    public class DeleteOrderDetailResponse
    {
        public bool Success { get; set; }
    }

} 