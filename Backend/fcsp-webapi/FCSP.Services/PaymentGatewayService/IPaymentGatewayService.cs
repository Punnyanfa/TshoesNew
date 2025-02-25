using FCSP.DTOs.PaymentGateway;
using FCSP.Models.Entities;

namespace FCSP.Services.PaymentGatewayService
{
    public interface IPaymentGatewayService
    {
        Task<IEnumerable<PaymentGateway>> GetAllPaymentGateways();
        Task<GetPaymentGatewayByIdResponse> GetPaymentGatewayById(GetPaymentGatewayByIdRequest request);
        Task<AddPaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request);
        Task<AddPaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request);
        Task<AddPaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request);
    }
} 