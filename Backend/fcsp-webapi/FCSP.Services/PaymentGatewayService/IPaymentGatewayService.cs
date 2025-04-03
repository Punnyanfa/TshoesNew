using FCSP.DTOs;
using FCSP.DTOs.PaymentGateway;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.PaymentGatewayService
{
    public interface IPaymentGatewayService
    {
        Task<PaymentGatewayListResponse> GetAllPaymentGateways();
        Task<PaymentGatewayResponse> GetPaymentGatewayById(GetPaymentGatewayByIdRequest request);
        Task<PaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request);
        Task<PaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request);
        Task<PaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request);
        Task<PaymentGatewayListResponse> GetPaymentGatewaysByUser(GetPaymentGatewaysByUserRequest request);
    }
} 