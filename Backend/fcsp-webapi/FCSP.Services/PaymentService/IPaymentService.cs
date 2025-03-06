using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<GetPaymentByIdResponse> GetPaymentById(GetPaymentByIdRequest request);
        Task<AddPaymentResponse> AddPayment(AddPaymentRequest request);
        Task<UpdatePaymentResponse> UpdatePayment(UpdatePaymentRequest request);
        Task<DeletePaymentResponse> DeletePayment(DeletePaymentRequest request);
    }
} 