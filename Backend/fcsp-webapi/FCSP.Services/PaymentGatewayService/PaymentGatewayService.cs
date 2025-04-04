using FCSP.DTOs;
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

        public async Task<PaymentGatewayListResponse> GetAllPaymentGateways()
        {
            try
            {
                var paymentGateways = await _paymentGatewayRepository.GetAllAsync();
                var paymentGatewayDtos = paymentGateways.Select(MapToDto).ToList();
                
                return new PaymentGatewayListResponse
                {
                    Code = 200,
                    Message = "Payment gateways retrieved successfully",
                    Data = paymentGatewayDtos
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayListResponse
                {
                    Code = 500,
                    Message = $"Error retrieving payment gateways: {ex.Message}"
                };
            }
        }

        public async Task<PaymentGatewayResponse> GetPaymentGatewayById(GetPaymentGatewayByIdRequest request)
        {
            try
            {
                var paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
                if (paymentGateway == null)
                {
                    return new PaymentGatewayResponse
                    {
                        Code = 404,
                        Message = "Payment gateway not found"
                    };
                }

                return new PaymentGatewayResponse
                {
                    Code = 200,
                    Message = "Payment gateway retrieved successfully",
                    Data = MapToDto(paymentGateway)
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayResponse
                {
                    Code = 500,
                    Message = $"Error retrieving payment gateway: {ex.Message}"
                };
            }
        }

        public async Task<PaymentGatewayResponse> AddPaymentGateway(AddPaymentGatewayRequest request)
        {
            try
            {
                PaymentGateway paymentGateway = GetEntityFromAddRequest(request);
                var addedPaymentGateway = await _paymentGatewayRepository.AddAsync(paymentGateway);
                
                return new PaymentGatewayResponse
                {
                    Code = 201,
                    Message = "Payment gateway created successfully",
                    Data = MapToDto(addedPaymentGateway)
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayResponse
                {
                    Code = 500,
                    Message = $"Error creating payment gateway: {ex.Message}"
                };
            }
        }

        public async Task<PaymentGatewayResponse> UpdatePaymentGateway(UpdatePaymentGatewayRequest request)
        {
            try
            {
                var paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
                if (paymentGateway == null)
                {
                    return new PaymentGatewayResponse
                    {
                        Code = 404,
                        Message = "Payment gateway not found"
                    };
                }

                // Parse payment method from request.Provider if needed
                if (Enum.TryParse<PaymentMethod>(request.Provider, true, out PaymentMethod method))
                {
                    paymentGateway.PaymentMethod = method;
                }
                paymentGateway.UpdatedAt = DateTime.UtcNow;

                await _paymentGatewayRepository.UpdateAsync(paymentGateway);
                
                return new PaymentGatewayResponse
                {
                    Code = 200,
                    Message = "Payment gateway updated successfully",
                    Data = MapToDto(paymentGateway)
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayResponse
                {
                    Code = 500,
                    Message = $"Error updating payment gateway: {ex.Message}"
                };
            }
        }

        public async Task<PaymentGatewayResponse> DeletePaymentGateway(DeletePaymentGatewayRequest request)
        {
            try
            {
                var paymentGateway = await _paymentGatewayRepository.FindAsync(request.Id);
                if (paymentGateway == null)
                {
                    return new PaymentGatewayResponse
                    {
                        Code = 404,
                        Message = "Payment gateway not found"
                    };
                }

                await _paymentGatewayRepository.DeleteAsync(paymentGateway.Id);
                
                return new PaymentGatewayResponse
                {
                    Code = 200,
                    Message = "Payment gateway deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayResponse
                {
                    Code = 500,
                    Message = $"Error deleting payment gateway: {ex.Message}"
                };
            }
        }

        public async Task<PaymentGatewayListResponse> GetPaymentGatewaysByUser(GetPaymentGatewaysByUserRequest request)
        {
            try
            {
                var paymentGateways = await _paymentGatewayRepository.GetByUserIdAsync(request.UserId);
                var paymentGatewayDtos = paymentGateways.Select(MapToDto).ToList();
                
                return new PaymentGatewayListResponse
                {
                    Code = 200,
                    Message = "Payment gateways retrieved successfully",
                    Data = paymentGatewayDtos
                };
            }
            catch (Exception ex)
            {
                return new PaymentGatewayListResponse
                {
                    Code = 500,
                    Message = $"Error retrieving payment gateways: {ex.Message}"
                };
            }
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

        private PaymentGatewayDto MapToDto(PaymentGateway paymentGateway)
        {
            return new PaymentGatewayDto
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
    }
} 