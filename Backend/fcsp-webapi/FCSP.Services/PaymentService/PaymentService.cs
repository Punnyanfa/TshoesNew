using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.TransactionService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace FCSP.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionService _transactionService;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PaymentService(IPaymentRepository paymentRepository,
                                IOrderRepository orderRepository,
                                IUserRepository userRepository,
                                ITransactionService transactionService,
                                IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _transactionService = transactionService;
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
                            Status = p.PaymentStatus,
                            PaymentMethod = p.PaymentMethod
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
                    var requestPayOS = new PayOSPaymentDTO
                    {
                        Id = payment.Id,
                        OrderId = payment.OrderId,
                        Amount = payment.Amount,
                        PaymentMessage = "Payment for order " + payment.OrderId
                    };
                    var response = await GetPayOSUrl(requestPayOS);
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
                    Data = new AddPaymentResponse
                    {
                        Response = string.Empty
                    },
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
                        Transactions = paymentInfo.transactions,
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

                if(payment.OrderId == 1 && payment.PaymentStatus == PaymentStatus.Received)
                {
                    var responsePayOS = await GetPaymentInfoFromPayOS(new GetPaymentInfoRequest { PaymentId = payment.Id });
                    var userId = responsePayOS.Data.Transactions.First().description.Split(" ")[8];
                    var transactionResponse = await _transactionService.UpdateBalanceAsync(new RechargeRequestDTO { UserId = long.Parse(userId), PaymentId = payment.Id, Amount = payment.Amount });

                    if (transactionResponse.Code != 200)
                    {
                        throw new Exception($"Payment failed: {transactionResponse.Message}");
                    }
                }
                else
                {
                    await UpdateOrderStatus(payment);
                }
                
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

        public async Task<BaseResponseModel<AddPaymentResponse>> RechargeBalanceAsync(RechargeRequestDTO request)
        {
            try
            {
                var user = await _userRepository.FindAsync(request.UserId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Create a payment record for recharge
                var payment = new Payment
                {
                    OrderId = 1, // No order for recharge
                    Amount = (int)request.Amount,
                    PaymentMethod = PaymentMethod.PayOS,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _paymentRepository.AddAsync(payment);

                var requestPayOS = new PayOSPaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    PaymentMessage = $"Recharge to user { user.Id } wallet"
                };
                var paymentResponse = await GetPayOSUrl(requestPayOS);

                return new BaseResponseModel<AddPaymentResponse>
                {
                    Code = 200,
                    Message = "Recharge payment created successfully",
                    Data = new AddPaymentResponse
                    {
                        Response = paymentResponse.checkoutUrl
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddPaymentResponse>
                {
                    Code = 500,
                    Message = $"Error creating recharge payment: {ex.Message}",
                    Data = new AddPaymentResponse
                    {
                        Response = string.Empty
                    }
                };
            }
        }

        public async Task<BaseResponseModel<WithdrawBalanceResponse>> WithdrawBalanceAsync(WithdrawRequestDTO request)
        {
            try
            {
                var user = await _userRepository.FindAsync(request.UserId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Check if user has sufficient balance
                if (user.Balance < request.Amount)
                {
                    throw new Exception("Insufficient balance");
                }

                var withdrawAmount = -request.Amount;

                var payment = new Payment
                {
                    OrderId = 1, // No order for recharge
                    Amount = (int)withdrawAmount,
                    PaymentMethod = PaymentMethod.Wallet,
                    PaymentStatus = PaymentStatus.Received,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _paymentRepository.AddAsync(payment);

                var transactionResponse = await _transactionService.UpdateBalanceAsync(new RechargeRequestDTO
                {   
                    UserId = request.UserId,
                    PaymentId = payment.Id,
                    Amount = payment.Amount 
                });

                if (transactionResponse.Code != 200)
                {
                    throw new Exception($"Transaction failed: {transactionResponse.Message}");
                }

                return new BaseResponseModel<WithdrawBalanceResponse>
                {
                    Code = 200,
                    Message = "Withdrawal created successfully",
                    Data = new WithdrawBalanceResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<WithdrawBalanceResponse>
                {
                    Code = 500,
                    Message = $"Error creating withdrawal: {ex.Message}",
                    Data = new WithdrawBalanceResponse
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

        private async Task<CreatePaymentResult> GetPayOSUrl(PayOSPaymentDTO payment)
        {   
            int expireAt = (int)DateTime.Now.AddMinutes(5).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
            PaymentData paymentData = new PaymentData(
                payment.Id,
                payment.Amount,
                payment.PaymentMessage,
                null,
                "https://tshoes.vercel.app/paymentCancelledPage",
                "https://tshoes.vercel.app/paymentSuccessPage",
                _checksumKey,
                null,
                null,
                null,
                null,
                expireAt
            );
            var paymentResponse = await payOS.createPaymentLink(paymentData);
            return paymentResponse;
        }

        private async Task<string> ProcessWalletPayment(Payment payment)
        {
            var paymentWithIncludes = await _paymentRepository.GetAll()
                                                            .Include(x => x.Order)
                                                            .ThenInclude(x => x.User)
                                                            .FirstOrDefaultAsync(p => p.Id == payment.Id);
            var user = await _userRepository.FindAsync(paymentWithIncludes.Order.UserId);
            var order = await _orderRepository.FindAsync(payment.OrderId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Balance < payment.Amount)
            {
                order.Status = OrderStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);
                payment.PaymentStatus = PaymentStatus.Cancelled;
                await _paymentRepository.UpdateAsync(payment);
                throw new Exception("Insufficient balance");
            }
            order.Status = OrderStatus.Confirmed;
            await _orderRepository.UpdateAsync(order);
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
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
            payment.UpdatedAt = DateTime.Now;
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
            order.UpdatedAt = DateTime.Now;
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
            payment.UpdatedAt = DateTime.Now;
            return payment;
        } 
        #endregion
    }
}