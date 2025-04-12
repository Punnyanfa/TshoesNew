using System;
using FCSP.Common.Enums;

namespace FCSP.DTOs.Voucher
{
    public class GetVoucherByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetVoucherByIdResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public float DiscountAmount { get; set; }
        public VoucherStatus Status { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public List<long> OrderIds { get; set; } = new List<long>();
    }

    public class GetVoucherByOrderIdRequest
    {
        public long OrderId { get; set; }
    }

    public class GetVoucherByOrderIdResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public VoucherStatus Status { get; set; }
    }

    public class AddVoucherRequest
    {
        public string Code { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class AddVoucherResponse
    {
        public long VoucherId { get; set; }
    }

    public class UpdateVoucherRequest
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
      
    }

    public class UpdateVoucherResponse
    {
        public long VoucherId { get; set; }
    }

    public class DeleteVoucherRequest
    {
        public long Id { get; set; }
    }

    public class DeleteVoucherResponse
    {
        public bool Success { get; set; }
    }
}