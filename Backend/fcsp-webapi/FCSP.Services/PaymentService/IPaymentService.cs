using FCSP.DTOs;
using FCSP.DTOs.Payment;

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
        Task<BaseResponseModel<ConfirmWebhookResponse>> ConfirmWebhook(ConfirmWebhookRequest request);
        Task<BaseResponseModel<UpdatePaymentUsingWebhookResponse>> UpdatePaymentUsingWebhook(UpdatePaymentUsingWebhookRequest request);
        Task<BaseResponseModel<AddPaymentResponse>> RechargeBalanceAsync(RechargeRequestDTO request);
    }
}