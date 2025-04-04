using FCSP.DTOs;
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

        public async Task<PaymentListResponse> GetAllPayments()
        {
            try
            {
                var payments = await _paymentRepository.GetAllAsync();
                var paymentDtos = payments.Select(MapToDto).ToList();
                
                return new PaymentListResponse
                {
                    Code = 200,
                    Message = "Payments retrieved successfully",
                    Data = paymentDtos
                };
            }
            catch (Exception ex)
            {
                return new PaymentListResponse
                {
                    Code = 500,
                    Message = $"Error retrieving payments: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResponse> GetPaymentById(GetPaymentByIdRequest request)
        {
            try
            {
                var payment = await _paymentRepository.FindAsync(request.Id);
                if (payment == null)
                {
                    return new PaymentResponse
                    {
                        Code = 404,
                        Message = "Payment not found"
                    };
                }

                return new PaymentResponse
                {
                    Code = 200,
                    Message = "Payment retrieved successfully",
                    Data = MapToDto(payment)
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Code = 500,
                    Message = $"Error retrieving payment: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResponse> AddPayment(AddPaymentRequest request)
        {
            try
            {
                Payment payment = GetEntityFromAddRequest(request);
                var addedPayment = await _paymentRepository.AddAsync(payment);
                
                return new PaymentResponse
                {
                    Code = 201,
                    Message = "Payment created successfully",
                    Data = MapToDto(addedPayment)
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Code = 500,
                    Message = $"Error creating payment: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResponse> UpdatePayment(UpdatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentRepository.FindAsync(request.Id);
                if (payment == null)
                {
                    return new PaymentResponse
                    {
                        Code = 404,
                        Message = "Payment not found"
                    };
                }

                payment.Amount = request.Amount;
                payment.PaymentStatus = (PaymentStatus)request.Status;
                payment.UpdatedAt = DateTime.UtcNow;

                await _paymentRepository.UpdateAsync(payment);
                
                return new PaymentResponse
                {
                    Code = 200,
                    Message = "Payment updated successfully",
                    Data = MapToDto(payment)
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Code = 500,
                    Message = $"Error updating payment: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResponse> DeletePayment(DeletePaymentRequest request)
        {
            try
            {
                var payment = await _paymentRepository.FindAsync(request.Id);
                if (payment == null)
                {
                    return new PaymentResponse
                    {
                        Code = 404,
                        Message = "Payment not found"
                    };
                }

                await _paymentRepository.DeleteAsync(payment.Id);
                
                return new PaymentResponse
                {
                    Code = 200,
                    Message = "Payment deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Code = 500,
                    Message = $"Error deleting payment: {ex.Message}"
                };
            }
        }

        private Payment GetEntityFromAddRequest(AddPaymentRequest request)
        {
            return new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = PaymentMethod.Card,
                PaymentStatus = (PaymentStatus)request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = (int)payment.PaymentStatus,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }
    }
} 