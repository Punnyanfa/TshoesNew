using FCSP.DTOs;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentListResponse> GetAllPayments();
        Task<PaymentResponse> GetPaymentById(GetPaymentByIdRequest request);
        Task<PaymentResponse> AddPayment(AddPaymentRequest request);
        Task<PaymentResponse> UpdatePayment(UpdatePaymentRequest request);
        Task<PaymentResponse> DeletePayment(DeletePaymentRequest request);
    }
} 