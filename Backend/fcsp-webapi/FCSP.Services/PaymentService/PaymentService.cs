using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FCSP.Common.Enums;

namespace FCSP.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            var response = await _paymentRepository.GetAllAsync();
            return response;
        }

        public async Task<GetPaymentByIdResponse> GetPaymentById(GetPaymentByIdRequest request)
        {
            Payment payment = await GetEntityFromGetByIdRequest(request);
            return new GetPaymentByIdResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = (int)payment.PaymentStatus,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }

        public async Task<AddPaymentResponse> AddPayment(AddPaymentRequest request)
        {
            Payment payment = GetEntityFromAddRequest(request);
            var addedPayment = await _paymentRepository.AddAsync(payment);
            return new AddPaymentResponse {
                Id = addedPayment.Id,
                Amount = addedPayment.Amount,
                Status = (int)addedPayment.PaymentStatus
            };
        }

        public async Task<UpdatePaymentResponse> UpdatePayment(UpdatePaymentRequest request)
        {
            Payment payment = await GetEntityFromUpdateRequest(request);
            await _paymentRepository.UpdateAsync(payment);
            return new UpdatePaymentResponse {
                Id = payment.Id,
                Amount = payment.Amount,
                Status = (int)payment.PaymentStatus
            };
        }

        public async Task<DeletePaymentResponse> DeletePayment(DeletePaymentRequest request)
        {
            Payment payment = await GetEntityFromDeleteRequest(request);
            await _paymentRepository.DeleteAsync(payment.Id);
            return new DeletePaymentResponse { Success = true };
        }

        private async Task<Payment> GetEntityFromGetByIdRequest(GetPaymentByIdRequest request)
        {
            Payment payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }
            return payment;
        }

        private Payment GetEntityFromAddRequest(AddPaymentRequest request)
        {
            return new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = (int)PaymentMethod.Card,
                PaymentStatus = (int)PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<Payment> GetEntityFromUpdateRequest(UpdatePaymentRequest request)
        {
            Payment payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }

            payment.Amount = request.Amount;
            payment.PaymentStatus = (int)PaymentStatus.Received;
            payment.UpdatedAt = DateTime.UtcNow;

            return payment;
        }

        private async Task<Payment> GetEntityFromDeleteRequest(DeletePaymentRequest request)
        {
            Payment payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }
            return payment;
        }
    }
} 