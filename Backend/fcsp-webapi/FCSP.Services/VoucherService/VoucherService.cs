using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

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
                ExpiryDate = voucher.ExpirationDate,
                IsUsed = voucher.Status == 1 // Assuming Status 1 means used
            };
        }

        public async Task<AddVoucherResponse> AddVoucher(AddVoucherRequest request)
        {
            var voucher = new Voucher
            {
                VoucherName = request.Code,
                VoucherValue = request.DiscountAmount.ToString(),
                ExpirationDate = request.ExpiryDate,
                Status = 0, // Not used
                Description = request.Code // Using code as description since there's no Description in the request
            };

            var addedVoucher = await _voucherRepository.AddAsync(voucher);
            return new AddVoucherResponse { VoucherId = addedVoucher.Id };
        }

        public async Task<UpdateVoucherResponse> UpdateVoucher(UpdateVoucherRequest request)
        {
            var voucher = await _voucherRepository.FindAsync(request.Id);
            if (voucher == null)
                throw new Exception("Voucher not found");
                
            voucher.VoucherName = request.Code;
            voucher.VoucherValue = request.DiscountAmount.ToString();
            voucher.ExpirationDate = request.ExpiryDate;
            voucher.Status = request.IsUsed ? 1 : 0;
            
            await _voucherRepository.UpdateAsync(voucher);
            
            return new UpdateVoucherResponse { VoucherId = voucher.Id };
        }

        public async Task<DeleteVoucherResponse> DeleteVoucher(DeleteVoucherRequest request)
        {
            await _voucherRepository.DeleteAsync(request.Id);
            return new DeleteVoucherResponse { Success = true };
        }
    }
} 