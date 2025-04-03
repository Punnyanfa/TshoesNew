using FCSP.DTOs;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;

namespace FCSP.Services.VoucherService
{
    public interface IVoucherService
    {
        Task<BaseResponseModel<List<Voucher>>> GetAllVouchers();
        Task<BaseResponseModel<GetVoucherByIdResponse>> GetVoucherById(GetVoucherByIdRequest request);
        Task<BaseResponseModel<GetVoucherByOrderIdResponse>> GetVoucherByOrderId(GetVoucherByOrderIdRequest request);
        Task<BaseResponseModel<AddVoucherResponse>> AddVoucher(AddVoucherRequest request);
        Task<BaseResponseModel<UpdateVoucherResponse>> UpdateVoucher(UpdateVoucherRequest request);
        Task<BaseResponseModel<DeleteVoucherResponse>> DeleteVoucher(DeleteVoucherRequest request);
        Task<BaseResponseModel<int>> UpdateExpiredVouchers();
        Task<BaseResponseModel<List<GetVoucherByIdResponse>>> GetNonExpiredVouchers();
    }
} 