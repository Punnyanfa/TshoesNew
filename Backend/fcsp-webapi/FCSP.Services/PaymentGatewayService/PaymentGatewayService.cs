using FCSP.DTOs.PaymentGateway;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FCSP.Common.Enums;

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
            PaymentGateway paymentGateway = await GetEntityFromGetByIdRequest(request);
            return new GetPaymentGatewayByIdResponse
            {
                Id = paymentGateway.Id,
                UserId = paymentGateway.UserId,
                Name = paymentGateway.PaymentMethod.ToString(),
                Provider = paymentGateway.PaymentMethod.ToString(),
                AccountNumber = "N/A", // Set appropriate value if available
                IsDefault = false, // Set appropriate value if available
                CreatedAt = paymentGateway.CreatedAt,
                UpdatedAt = paymentGateway.UpdatedAt
            };
        }

        public async Task<AddPaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = GetEntityFromAddRequest(request);
            var addedPaymentGateway = await _paymentGatewayRepository.AddAsync(paymentGateway);
            return new AddPaymentGatewayResponse 
            { 
                Id = addedPaymentGateway.Id,
                Name = addedPaymentGateway.PaymentMethod.ToString(),
                IsDefault = false // Set appropriate value if available
            };
        }

        public async Task<UpdatePaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = await GetEntityFromUpdateRequest(request);
            await _paymentGatewayRepository.UpdateAsync(paymentGateway);
            return new UpdatePaymentGatewayResponse 
            { 
                Id = paymentGateway.Id,
                Name = paymentGateway.PaymentMethod.ToString(),
                IsDefault = false // Set appropriate value if available
            };
        }

        public async Task<DeletePaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = await GetEntityFromDeleteRequest(request);
            await _paymentGatewayRepository.DeleteAsync(paymentGateway.Id);
            return new DeletePaymentGatewayResponse { Success = true };
        }

        private async Task<PaymentGateway> GetEntityFromGetByIdRequest(GetPaymentGatewayByIdRequest request)
        {
            PaymentGateway paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
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
                PaymentMethod = PaymentMethod.Card,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<PaymentGateway> GetEntityFromUpdateRequest(UpdatePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("PaymentGateway not found");
            }
            
            // Parse payment method from request.Provider if needed
            if (Enum.TryParse<PaymentMethod>(request.Provider, true, out PaymentMethod method))
            {
                paymentGateway.PaymentMethod = method;
            }
            paymentGateway.UpdatedAt = DateTime.UtcNow;
            
            return paymentGateway;
        }

        private async Task<PaymentGateway> GetEntityFromDeleteRequest(DeletePaymentGatewayRequest request)
        {
            PaymentGateway paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("PaymentGateway not found");
            }
            return paymentGateway;
        }
    }
} 