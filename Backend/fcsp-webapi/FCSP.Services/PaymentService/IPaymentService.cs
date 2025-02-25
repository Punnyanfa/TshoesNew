using FCSP.DTOs.Payment;
using FCSP.Models.Entities;

namespace FCSP.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<GetPaymentByIdResponse> GetPaymentById(GetPaymentByIdRequest request);
        Task<AddPaymentResponse> AddPayment(AddPaymentRequest request);
        Task<AddPaymentResponse> UpdatePayment(UpdatePaymentRequest request);
        Task<AddPaymentResponse> DeletePayment(DeletePaymentRequest request);
    }
} 