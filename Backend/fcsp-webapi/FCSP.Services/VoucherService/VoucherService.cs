using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Common.Enums;

namespace FCSP.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private const int MAX_EXPIRY_DAYS = 30;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchers()
        {
            return await _voucherRepository.GetAllAsync();
        }

        public async Task<GetVoucherByIdResponse> GetVoucherById(GetVoucherByIdRequest request)
        {
            var voucher = await _voucherRepository.FindAsync(request.Id);
            if (voucher == null)
                throw new Exception("Voucher not found");

            return new GetVoucherByIdResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                Status = (VoucherStatus)voucher.Status,
                ExpiryDate = voucher.ExpirationDate,
                IsUsed = voucher.Status == (int)VoucherStatus.Used
            };
        }

        public async Task<GetVoucherByOrderIdResponse> GetVoucherByOrderId(GetVoucherByOrderIdRequest request)
        {
            var voucher = await _voucherRepository.GetVoucherByOrderIdAsync(request.OrderId);
            if (voucher == null)
                throw new Exception("No voucher found for this order");

            return new GetVoucherByOrderIdResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                ExpiryDate = voucher.ExpirationDate,
                Status = (VoucherStatus)voucher.Status
            };
        }

        public async Task<AddVoucherResponse> AddVoucher(AddVoucherRequest request)
        {
            ValidateExpirationDate(request.ExpiryDate);

            var voucher = new Voucher
            {
                VoucherName = request.Code,
                VoucherValue = request.DiscountAmount.ToString(),
                ExpirationDate = request.ExpiryDate,
                Status = (int)VoucherStatus.Active,
                Description = request.Code
                // CreatedAt is set by BaseEntity
            };

            var addedVoucher = await _voucherRepository.AddAsync(voucher);
            return new AddVoucherResponse { VoucherId = addedVoucher.Id };
        }

        public async Task<UpdateVoucherResponse> UpdateVoucher(UpdateVoucherRequest request)
        {
            var voucher = await _voucherRepository.FindAsync(request.Id);
            if (voucher == null)
                throw new Exception("Voucher not found");

            ValidateExpirationDate(request.ExpiryDate, voucher.CreatedAt);

            voucher.VoucherName = request.Code;
            voucher.VoucherValue = request.DiscountAmount.ToString();
            voucher.ExpirationDate = request.ExpiryDate;
            voucher.Status = request.IsUsed ? (int)VoucherStatus.Used : (int)VoucherStatus.Active;
            voucher.UpdatedAt = DateTime.Now;

            await _voucherRepository.UpdateAsync(voucher);
            return new UpdateVoucherResponse { VoucherId = voucher.Id };
        }

        public async Task<DeleteVoucherResponse> DeleteVoucher(DeleteVoucherRequest request)
        {
            await _voucherRepository.DeleteAsync(request.Id);
            return new DeleteVoucherResponse { Success = true };
        }

        public async Task<int> UpdateExpiredVouchers()
        {
           return await _voucherRepository.UpdateExpiredVouchersAsync();
        }

        private void ValidateExpirationDate(DateTime expiryDate, DateTime? createdAt = null)
        {
            var now = DateTime.UtcNow;
            createdAt ??= now;

            if (expiryDate < now)
                throw new ArgumentException("Expiration date cannot be in the past");

            var maxExpiryDate = createdAt.Value.AddDays(MAX_EXPIRY_DAYS);
            if (expiryDate > maxExpiryDate)
                throw new ArgumentException($"Expiration date cannot be more than {MAX_EXPIRY_DAYS} days from creation date"); // Corrected from 'Facetiming'
        }
    }
}