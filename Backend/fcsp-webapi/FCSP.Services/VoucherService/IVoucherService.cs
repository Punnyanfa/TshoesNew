using FCSP.DTOs;
using FCSP.DTOs.Voucher;

namespace FCSP.Services.VoucherService
{
    public interface IVoucherService
    {
        Task<BaseResponseModel<List<GetAllVoucherResponse>>> GetAllVouchers();
        Task<BaseResponseModel<GetVoucherByCodeResponse>> GetVoucherByCode(GetVoucherByCodeRequest request);
        Task<BaseResponseModel<GetVoucherByIdResponse>> GetVoucherById(GetVoucherByIdRequest request);
        Task<BaseResponseModel<GetVoucherByOrderIdResponse>> GetVoucherByOrderId(GetVoucherByOrderIdRequest request);
        Task<BaseResponseModel<AddVoucherResponse>> AddVoucher(AddVoucherRequest request);
        Task<BaseResponseModel<UpdateVoucherResponse>> UpdateVoucher(UpdateVoucherRequest request);
        Task<BaseResponseModel<DeleteVoucherResponse>> DeleteVoucher(DeleteVoucherRequest request);
        Task<BaseResponseModel<int>> UpdateExpiredVouchers();
        Task<BaseResponseModel<List<GetVoucherByIdResponse>>> GetNonExpiredVouchers();
    }
}