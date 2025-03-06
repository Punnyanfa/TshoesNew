using FCSP.DTOs.PaymentGateway;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.PaymentGatewayService
{
    public interface IPaymentGatewayService
    {
        Task<IEnumerable<PaymentGateway>> GetAllPaymentGateways();
        Task<GetPaymentGatewayByIdResponse> GetPaymentGatewayById(GetPaymentGatewayByIdRequest request);
        Task<AddPaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request);
        Task<UpdatePaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request);
        Task<DeletePaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request);
    }
} 