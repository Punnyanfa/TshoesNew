using System;
using System.Collections.Generic;

namespace FCSP.DTOs.ReturnedCustomShoe
{
    // Request/Response models for adding returned custom shoes
    public class AddReturnedCustomShoeRequest
    {
        public long CustomShoeDesignId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }

    public class AddReturnedCustomShoeResponse
    {
        public bool Success { get; set; }
    }

    // Request/Response models for getting returned custom shoe by ID
    public class GetReturnedCustomShoeByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetReturnedCustomShoeByIdResponse
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Response model for getting all returned custom shoes
    public class GetReturnedCustomShoesResponse
    {
        public IEnumerable<ReturnedCustomShoeDto> ReturnedShoes { get; set; }
    }

    // DTO for returned custom shoe
    public class ReturnedCustomShoeDto
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 