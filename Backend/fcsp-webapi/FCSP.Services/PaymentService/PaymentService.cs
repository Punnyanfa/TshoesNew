using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

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
            Payment payment = GetEntityFromGetByIdRequest(request);
            return new GetPaymentByIdResponse
            {
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus
            };
        }

        public async Task<AddPaymentResponse> AddPayment(AddPaymentRequest request)
        {
            Payment payment = GetEntityFromAddRequest(request);
            var addedPayment = await _paymentRepository.AddAsync(payment);
            return new AddPaymentResponse { PaymentId = addedPayment.Id };
        }

        public async Task<AddPaymentResponse> UpdatePayment(UpdatePaymentRequest request)
        {
            Payment payment = GetEntityFromUpdateRequest(request);
            await _paymentRepository.UpdateAsync(payment);
            return new AddPaymentResponse { PaymentId = payment.Id };
        }

        public async Task<AddPaymentResponse> DeletePayment(DeletePaymentRequest request)
        {
            Payment payment = GetEntityFromDeleteRequest(request);
            await _paymentRepository.DeleteAsync(payment.Id);
            return new AddPaymentResponse { PaymentId = payment.Id };
        }

        private Payment GetEntityFromGetByIdRequest(GetPaymentByIdRequest request)
        {
            Payment payment = _paymentRepository.Find(request.Id);
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
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = request.PaymentStatus
            };
        }

        private Payment GetEntityFromUpdateRequest(UpdatePaymentRequest request)
        {
            Payment payment = _paymentRepository.Find(request.Id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }
            
            payment.OrderId = request.OrderId ?? payment.OrderId;
            payment.Amount = request.Amount ?? payment.Amount;
            payment.PaymentMethod = request.PaymentMethod ?? payment.PaymentMethod;
            payment.PaymentStatus = request.PaymentStatus ?? payment.PaymentStatus;
            payment.UpdatedAt = DateTime.Now;
            
            return payment;
        }

        private Payment GetEntityFromDeleteRequest(DeletePaymentRequest request)
        {
            Payment payment = _paymentRepository.Find(request.Id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }
            return payment;
        }
    }
} 