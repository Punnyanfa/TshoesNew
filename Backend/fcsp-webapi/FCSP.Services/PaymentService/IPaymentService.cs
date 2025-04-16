using FCSP.DTOs;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<BaseResponseModel<AddPaymentResponse>> TestPayOSAsync(AddPaymentRequest request);
        Task<BaseResponseModel<GetPaymentInfoResponse>> GetPaymentInfoFromPayOS(GetPaymentInfoRequest request);
        Task<BaseResponseModel<PaymentListResponse>> GetAllPayments();
        Task<BaseResponseModel<GetPaymentByIdResponse>> GetPaymentById(GetPaymentByIdRequest request);
        Task<BaseResponseModel<AddPaymentResponse>> AddPayment(AddPaymentRequest request);
        Task<BaseResponseModel<UpdatePaymentResponse>> UpdatePayment(UpdatePaymentRequest request);
        Task<BaseResponseModel<CancelPaymentResponse>> CancelPaymentFromPayOS(CancelPaymentRequest request);
    }
} 