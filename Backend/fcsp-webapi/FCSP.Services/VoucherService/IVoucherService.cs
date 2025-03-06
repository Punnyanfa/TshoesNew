using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;

namespace FCSP.Services.VoucherService
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAllVouchers();
        Task<GetVoucherByIdResponse> GetVoucherById(GetVoucherByIdRequest request);
        Task<AddVoucherResponse> AddVoucher(AddVoucherRequest request);
        Task<UpdateVoucherResponse> UpdateVoucher(UpdateVoucherRequest request);
        Task<DeleteVoucherResponse> DeleteVoucher(DeleteVoucherRequest request);
    }
} 