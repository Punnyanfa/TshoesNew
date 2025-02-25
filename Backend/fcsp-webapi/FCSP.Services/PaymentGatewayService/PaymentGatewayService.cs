using FCSP.DTOs.PaymentGateway;
using FCSP.Models.Entities;
using FCSP.Repositories.PaymentGateway;

namespace FCSP.Services.PaymentGatewayService
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IPaymentGatewayRepository _paymentGatewayRepository;

        public PaymentGatewayService(IPaymentGatewayRepository paymentGatewayRepository)
        {
            _paymentGatewayRepository = paymentGatewayRepository;
        }

        public async Task<IEnumerable<PaymentGateway>> GetAllPaymentGateways()
        {
            var response = await _paymentGatewayRepository.GetAllAsync();
            return response;
        }

        public async Task<GetPaymentGatewayByIdResponse> GetPaymentGatewayById(GetPaymentGatewayByIdRequest request)
        {
            PaymentGateway paymentGateway = GetEntityFromGetByIdRequest(request);
            return new GetPaymentGatewayByIdResponse
            {
                UserId = paymentGateway.UserId,
                PaymentMethod = paymentGateway.PaymentMethod
            };
        }

        public async Task<AddPaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = GetEntityFromAddRequest(request);
            var addedPaymentGateway = await _paymentGatewayRepository.AddAsync(paymentGateway);
            return new AddPaymentGatewayResponse { PaymentGatewayId = addedPaymentGateway.Id };
        }

        public async Task<AddPaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = GetEntityFromUpdateRequest(request);
            await _paymentGatewayRepository.UpdateAsync(paymentGateway);
            return new AddPaymentGatewayResponse { PaymentGatewayId = paymentGateway.Id };
        }

        public async Task<AddPaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = GetEntityFromDeleteRequest(request);
            await _paymentGatewayRepository.DeleteAsync(paymentGateway.Id);
            return new AddPaymentGatewayResponse { PaymentGatewayId = paymentGateway.Id };
        }

        private PaymentGateway GetEntityFromGetByIdRequest(GetPaymentGatewayByIdRequest request)
        {
            PaymentGateway paymentGateway = _paymentGatewayRepository.Find(request.Id);
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("PaymentGateway not found");
            }
            return paymentGateway;
        }

        private PaymentGateway GetEntityFromAddRequest(AddPaymentGatewayRequest request)
        {
            return new PaymentGateway
            {
                UserId = request.UserId,
                PaymentMethod = request.PaymentMethod
            };
        }

        private PaymentGateway GetEntityFromUpdateRequest(UpdatePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = _paymentGatewayRepository.Find(request.Id);
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("PaymentGateway not found");
            }
            
            paymentGateway.UserId = request.UserId ?? paymentGateway.UserId;
            paymentGateway.PaymentMethod = request.PaymentMethod ?? paymentGateway.PaymentMethod;
            paymentGateway.UpdatedAt = DateTime.Now;
            
            return paymentGateway;
        }

        private PaymentGateway GetEntityFromDeleteRequest(DeletePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = _paymentGatewayRepository.Find(request.Id);
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("PaymentGateway not found");
            }
            return paymentGateway;
        }
    }
} 