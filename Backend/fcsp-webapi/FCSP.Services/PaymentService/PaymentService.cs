using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;

namespace FCSP.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _clientId = configuration["PayOS:ClientId"] ?? string.Empty;
            _apiKey = configuration["PayOS:ApiKey"] ?? string.Empty;
            _checksumKey = configuration["PayOS:ChecksumKey"] ?? string.Empty;
        }

        #region Public Methods
        public async Task<BaseResponseModel<AddPaymentResponse>> TestPayOSAsync(AddPaymentRequest request)
        {
            var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
            int expireAt = (int)DateTime.Now.AddMinutes(5).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            PaymentData paymentData = new PaymentData(request.OrderId, request.Amount, "Payment for order " + request.OrderId, null, "https://tshoes.vercel.app/paymentSuccessPage", "https://tshoes.vercel.app/paymentCancelledPage");
            var paymentResponse = await payOS.createPaymentLink(paymentData);
            return new BaseResponseModel<AddPaymentResponse>
            {
                Code = 200,
                Message = "Payment created successfully",
                Data = new AddPaymentResponse
                {
                    Response = paymentResponse.checkoutUrl
                }
            };
        }

        public async Task<BaseResponseModel<PaymentListResponse>> GetAllPayments()
        {
            try
            {
                var payments = await GetAllPaymentsFromRepository();

                return new BaseResponseModel<PaymentListResponse>
                {
                    Code = 200,
                    Message = "Payments retrieved successfully",
                    Data = new PaymentListResponse
                    {
                        Payments = payments.Select(p => new GetPaymentByIdResponse
                        {
                            Id = p.Id,
                            OrderId = p.OrderId,
                            Amount = p.Amount,
                            Status = p.PaymentStatus
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<PaymentListResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving payments: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<GetPaymentByIdResponse>> GetPaymentById(GetPaymentByIdRequest request)
        {
            try
            {
                var payment = await GetEntityFromGetByIdRequest(request);

                return new BaseResponseModel<GetPaymentByIdResponse>
                {
                    Code = 200,
                    Message = "Payment retrieved successfully",
                    Data = new GetPaymentByIdResponse
                    {
                        Id = payment.Id,
                        OrderId = payment.OrderId,
                        Amount = payment.Amount,
                        Status = payment.PaymentStatus
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetPaymentByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving payment: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddPaymentResponse>> AddPayment(AddPaymentRequest request)
        {
            try
            {
                var payment = GetEntityFromAddRequest(request);
                await _paymentRepository.AddAsync(payment);
                if (request.PaymentMethod == PaymentMethod.PayOS)
                {
                    var response = await GetPayOSUrl(payment);
                    return new BaseResponseModel<AddPaymentResponse>
                    {
                        Code = 200,
                        Message = "Payment created successfully",
                        Data = new AddPaymentResponse
                        {
                            Response = response.checkoutUrl
                        }
                    };
                }
                else if (request.PaymentMethod == PaymentMethod.Wallet)
                {
                    var response = await ProcessWalletPayment(payment);
                    return new BaseResponseModel<AddPaymentResponse>
                    {
                        Code = 200,
                        Message = "Payment created successfully",
                        Data = new AddPaymentResponse
                        {
                            Response = response
                        }
                    };
                }
                else
                {
                    throw new Exception("Payment method not supported");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddPaymentResponse>
                {
                    Code = 500,
                    Message = $"Error creating payment: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<UpdatePaymentResponse>> UpdatePayment(UpdatePaymentRequest request)
        {
            try
            {
                var payment = await GetEntityFromUpdateRequest(request);
                await _paymentRepository.UpdateAsync(payment);

                await UpdateOrderStatus(payment);

                return new BaseResponseModel<UpdatePaymentResponse>
                {
                    Code = 200,
                    Message = "Payment updated successfully",
                    Data = new UpdatePaymentResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdatePaymentResponse>
                {
                    Code = 500,
                    Message = $"Error updating payment: {ex.Message}",
                    Data = new UpdatePaymentResponse
                    {
                        Success = false
                    }
                };
            }
        }

        public async Task<BaseResponseModel<GetPaymentInfoResponse>> GetPaymentInfoFromPayOS(GetPaymentInfoRequest request)
        {
            try
            {
                var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
                var paymentInfo = await payOS.getPaymentLinkInformation(request.PaymentId);
                return new BaseResponseModel<GetPaymentInfoResponse>
                {
                    Code = 200,
                    Message = "Payment info retrieved successfully",
                    Data = new GetPaymentInfoResponse
                    {
                        PaymentId = paymentInfo.orderCode,
                        Status = paymentInfo.status,
                        Amount = paymentInfo.amount,
                        AmountPaid = paymentInfo.amountPaid,
                        AmountRemaining = paymentInfo.amountRemaining,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetPaymentInfoResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving payment info: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<CancelPaymentResponse>> CancelPaymentFromPayOS(CancelPaymentRequest request)
        {
            try
            {
                var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
                await payOS.cancelPaymentLink(request.PaymentId);
                return new BaseResponseModel<CancelPaymentResponse>
                {
                    Code = 200,
                    Message = "Payment cancelled successfully",
                    Data = new CancelPaymentResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<CancelPaymentResponse>
                {
                    Code = 500,
                    Message = $"Error cancelling payment: {ex.Message}",
                    Data = new CancelPaymentResponse
                    {
                        Success = false
                    }
                };
            }
        }
        
        public async Task<BaseResponseModel<ConfirmWebhookResponse>> ConfirmWebhook(ConfirmWebhookRequest request)
        {
            try
            {
                var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
                await payOS.confirmWebhook(request.WebhookUrl);
                return new BaseResponseModel<ConfirmWebhookResponse>
                {
                    Code = 200,
                    Message = "Webhook confirmed successfully",
                    Data = new ConfirmWebhookResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<ConfirmWebhookResponse>
                {
                    Code = 500,
                    Message = $"Error confirming webhook: {ex.Message}",
                    Data = new ConfirmWebhookResponse
                    {
                        Success = false
                    }
                };
            }
        }

        public async Task<BaseResponseModel<UpdatePaymentUsingWebhookResponse>> UpdatePaymentUsingWebhook(UpdatePaymentUsingWebhookRequest request)
        {
            try
            {
                var payment = await GetEntityFromUpdatePaymentRequest(request);
                await _paymentRepository.UpdateAsync(payment);

                await UpdateOrderStatus(payment);
                return new BaseResponseModel<UpdatePaymentUsingWebhookResponse>
                {
                    Code = 200,
                    Message = "Payment updated successfully",
                    Data = new UpdatePaymentUsingWebhookResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdatePaymentUsingWebhookResponse>
                {
                    Code = 500,
                    Message = $"Error updating payment: {ex.Message}",
                    Data = new UpdatePaymentUsingWebhookResponse
                    {
                        Success = false 
                    }
                };
            }
        }
        #endregion

        #region Private Methods

        private async Task<IEnumerable<Payment>> GetAllPaymentsFromRepository()
        {
            var payments = await _paymentRepository.GetAllAsync();
            if (payments == null || !payments.Any())
            {
                throw new Exception("No payments found");
            }
            return payments;
        }

        private async Task<Payment> GetEntityFromGetByIdRequest(GetPaymentByIdRequest request)
        {
            var payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new Exception("Payment not found");
            }
            return payment;
        }

        private async Task<CreatePaymentResult> GetPayOSUrl(Payment payment)
        {
            var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
            PaymentData paymentData = new PaymentData(
                payment.Id,
                payment.Amount,
                "Payment for order " + payment.OrderId,
                null,
                "https://tshoes.vercel.app/paymentSuccessPage",
                "https://tshoes.vercel.app/paymentCancelledPage"
            );
            var paymentResponse = await payOS.createPaymentLink(paymentData);
            return paymentResponse;
        }

        private async Task<string> ProcessWalletPayment(Payment payment)
        {
            var paymentWithIncludes = await _paymentRepository.GetAll().Include(x => x.Order).ThenInclude(x => x.User).FirstOrDefaultAsync(p => p.Id == payment.Id);
            var user = await _userRepository.FindAsync(paymentWithIncludes.Order.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Balance < payment.Amount)
            {
                throw new Exception("Insufficient balance");
            }
            user.Balance -= payment.Amount;
            await _userRepository.UpdateAsync(user);
            payment.PaymentStatus = PaymentStatus.Received;
            await _paymentRepository.UpdateAsync(payment);
            return "Payment processed successfully";
        }
        private Payment GetEntityFromAddRequest(AddPaymentRequest request)
        {
            return new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<Payment> GetEntityFromUpdateRequest(UpdatePaymentRequest request)
        {
            var payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new Exception("Payment not found");
            }
            if (request.Status == "CANCELLED")
            {
                payment.PaymentStatus = PaymentStatus.Cancelled;
            }
            else if (request.Status == "PAID")
            {
                payment.PaymentStatus = PaymentStatus.Received;
            }
            else if (request.Status == "PROCESSING")
            {
                payment.PaymentStatus = PaymentStatus.Pending;
            }
            else
            {
                throw new Exception("Invalid payment status");
            }
            payment.UpdatedAt = DateTime.UtcNow;
            return payment;
        }

        private async Task UpdateOrderStatus(Payment payment)
        {
            var order = await _orderRepository.FindAsync(payment.OrderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (payment.PaymentStatus == PaymentStatus.Cancelled)
            {
                order.Status = OrderStatus.Cancelled;
            }
            else if (payment.PaymentStatus == PaymentStatus.Received)
            {
                order.Status = OrderStatus.Confirmed;
            }
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
        }

        private async Task<Payment> GetEntityFromUpdatePaymentRequest(UpdatePaymentUsingWebhookRequest request)
        {
            var payment = await _paymentRepository.FindAsync(request.Id);
            if (payment == null)
            {
                throw new Exception("Payment not found");
            }
            
            if (request.Status == "CANCELLED")
            {
                payment.PaymentStatus = PaymentStatus.Cancelled;
            }
            else if (request.Status == "PAID")
            {
                payment.PaymentStatus = PaymentStatus.Received;
            }
            else if (request.Status == "PROCESSING")
            {
                payment.PaymentStatus = PaymentStatus.Pending;
            }
            else
            {
                throw new Exception("Invalid payment status");
            }
            payment.UpdatedAt = DateTime.UtcNow;
            return payment;
        } 
        #endregion
    }
}