using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IPaymentRepository paymentRepository,
            IVoucherRepository voucherRepository,
            IPaymentProcessor paymentProcessor,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _paymentRepository = paymentRepository;
            _voucherRepository = voucherRepository;
            _paymentProcessor = paymentProcessor;
            _logger = logger;
        }

        #region Public Methods

        public async Task<BaseResponseModel<List<GetOrderByUserIdResponse>>> GetOrdersByUserId(GetOrdersByUserIdRequest request)
        {
            try
            {
                _logger.LogInformation("Retrieving orders for user ID: {UserId}", request.UserId);
                var orders = await GetOrdersByUserIdAsync(request);
                _logger.LogInformation("Successfully retrieved {OrderCount} orders for user ID: {UserId}", orders.Count, request.UserId);
                return new BaseResponseModel<List<GetOrderByUserIdResponse>>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user ID: {UserId}", request.UserId);
                return HandleException<List<GetOrderByUserIdResponse>>(ex, "retrieving orders by user ID");
            }
        }

        public async Task<BaseResponseModel<GetOrderByIdResponse>> GetOrderById(GetOrderByIdRequest request)
        {
            try
            {
                _logger.LogInformation("Retrieving order with ID: {OrderId}", request.Id);
                var order = await GetOrderByIdAsync(request);
                _logger.LogInformation("Successfully retrieved order with ID: {OrderId}", request.Id);
                return new BaseResponseModel<GetOrderByIdResponse>
                {
                    Code = 200,
                    Message = "Order retrieved successfully",
                    Data = order
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order with ID: {OrderId}", request.Id);
                return HandleException<GetOrderByIdResponse>(ex, "retrieving order by ID");
            }
        }

        public async Task<BaseResponseModel<List<GetOrderByIdResponse>>> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("Retrieving all orders");
                var orders = await GetAllOrdersAsync();
                _logger.LogInformation("Successfully retrieved {OrderCount} orders", orders.Count);
                return new BaseResponseModel<List<GetOrderByIdResponse>>
                {
                    Code = 200,
                    Message = "All orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all orders");
                return HandleException<List<GetOrderByIdResponse>>(ex, "retrieving all orders");
            }
        }

        public async Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Adding new order for user ID: {UserId}", request.UserId);

                // Validate TotalPrice matches OrderDetails
                var calculatedTotal = CalculateOrderDetailsTotal(request.OrderDetails);
                if (Math.Abs(calculatedTotal - request.TotalPrice) > 0.01) // Cho phép sai số nhỏ do float
                {
                    _logger.LogWarning("TotalPrice {TotalPrice} does not match calculated total {CalculatedTotal} for user ID: {UserId}", request.TotalPrice, calculatedTotal, request.UserId);
                    return new BaseResponseModel<AddOrderResponse>
                    {
                        Code = 400,
                        Message = $"TotalPrice ({request.TotalPrice}) does not match the calculated total of OrderDetails ({calculatedTotal}).",
                        Data = null
                    };
                }

                // Validate Voucher (nếu có)
                float totalPrice = request.TotalPrice;
                if (request.VoucherId.HasValue)
                {
                    var voucher = await _voucherRepository.FindByIdAsync(request.VoucherId.Value);
                    if (voucher == null)
                    {
                        _logger.LogWarning("Voucher with ID {VoucherId} not found for user ID: {UserId}", request.VoucherId, request.UserId);
                        return new BaseResponseModel<AddOrderResponse>
                        {
                            Code = 400,
                            Message = "Voucher not found.",
                            Data = null
                        };
                    }

                    var (isValid, validationMessage) = IsVoucherValid(voucher);
                    if (!isValid)
                    {
                        _logger.LogWarning("Invalid voucher with ID {VoucherId} for user ID: {UserId}. Reason: {ValidationMessage}", request.VoucherId, request.UserId, validationMessage);
                        return new BaseResponseModel<AddOrderResponse>
                        {
                            Code = 400,
                            Message = $"Invalid voucher: {validationMessage}. Proceeding without voucher.",
                            Data = null
                        };
                    }

                    totalPrice = ApplyVoucherDiscount(totalPrice, voucher);
                }

                // Tạo Order
                var order = new Order
                {
                    UserId = request.UserId,
                    ShippingInfoId = request.ShippingInfoId,
                    VoucherId = request.VoucherId,
                    TotalPrice = totalPrice,
                    AmountPaid = 0,
                    Status = OrderStatus.Pending,
                    ShippingStatus = OrderShippingStatus.Preparing,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedOrder = await _orderRepository.AddAsync(order);
                _logger.LogInformation("Created order with ID: {OrderId} for user ID: {UserId}", addedOrder.Id, request.UserId);

                // Tạo OrderDetails
                foreach (var od in request.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = addedOrder.Id,
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        SizeId = od.SizeId,
                        Quantity = od.Quantity,
                        Price = od.UnitPrice,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _orderDetailRepository.AddAsync(orderDetail);
                }

                // Tạo Payment
                var payment = new Payment
                {
                    OrderId = addedOrder.Id,
                    Amount = addedOrder.TotalPrice,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _paymentRepository.AddAsync(payment);
                _logger.LogInformation("Created payment for order ID: {OrderId}", addedOrder.Id);

                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 201,
                    Message = "Order added successfully",
                    Data = new AddOrderResponse
                    {
                        Id = addedOrder.Id,
                        TotalPrice = addedOrder.TotalPrice,
                        Status = addedOrder.Status
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order for user ID: {UserId}", request.UserId);
                return HandleException<AddOrderResponse>(ex, "adding order");
            }
        }

        public async Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Updating order with ID: {OrderId}", request.Id);

                // Validate TotalPrice matches OrderDetails
                var calculatedTotal = CalculateOrderDetailsTotal(request.OrderDetails);
                if (Math.Abs(calculatedTotal - request.TotalPrice) > 0.01) // Cho phép sai số nhỏ do float
                {
                    _logger.LogWarning("TotalPrice {TotalPrice} does not match calculated total {CalculatedTotal} for order ID: {OrderId}", request.TotalPrice, calculatedTotal, request.Id);
                    return new BaseResponseModel<UpdateOrderResponse>
                    {
                        Code = 400,
                        Message = $"TotalPrice ({request.TotalPrice}) does not match the calculated total of OrderDetails ({calculatedTotal}).",
                        Data = null
                    };
                }

                var updatedOrder = await UpdateOrderAsync(request);
                _logger.LogInformation("Successfully updated order with ID: {OrderId}", request.Id);
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 200,
                    Message = "Order updated successfully",
                    Data = updatedOrder
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order with ID: {OrderId}", request.Id);
                return HandleException<UpdateOrderResponse>(ex, "updating order");
            }
        }

        public async Task<BaseResponseModel<DeleteOrderResponse>> DeleteOrder(DeleteOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Deleting order with ID: {OrderId}", request.Id);

                var order = await _orderRepository.FindAsync(request.Id);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", request.Id);
                    return new BaseResponseModel<DeleteOrderResponse>
                    {
                        Code = 404,
                        Message = "Order not found",
                        Data = new DeleteOrderResponse { Success = false }
                    };
                }

                // Kiểm tra trạng thái để cho phép hủy
                if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
                {
                    _logger.LogWarning("Order with ID {OrderId} cannot be cancelled in its current state: {Status}", request.Id, order.Status);
                    return new BaseResponseModel<DeleteOrderResponse>
                    {
                        Code = 400,
                        Message = "Order cannot be cancelled in its current state",
                        Data = new DeleteOrderResponse { Success = false }
                    };
                }

                // Nếu đã thanh toán, bắt đầu quy trình hoàn tiền
                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault(p => p.PaymentStatus == PaymentStatus.Received);
                if (payment != null)
                {
                    order.Status = OrderStatus.Refunded;
                    payment.PaymentStatus = PaymentStatus.Rejected; // Giả lập hoàn tiền
                    await _paymentRepository.UpdateAsync(payment);
                    _logger.LogInformation("Refunded payment for order ID: {OrderId}", order.Id);
                }
                else
                {
                    order.Status = OrderStatus.Cancelled;
                }

                order.ShippingStatus = OrderShippingStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);
                _logger.LogInformation("Successfully cancelled order with ID: {OrderId}", order.Id);

                return new BaseResponseModel<DeleteOrderResponse>
                {
                    Code = 200,
                    Message = "Order cancelled successfully",
                    Data = new DeleteOrderResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID: {OrderId}", request.Id);
                return HandleException<DeleteOrderResponse>(ex, "deleting order");
            }
        }

        public async Task<BaseResponseModel<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request)
        {
            try
            {
                _logger.LogInformation("Processing payment for order ID: {OrderId}", request.OrderId);

                var order = await _orderRepository.FindAsync(request.OrderId);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", request.OrderId);
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }

                if (order.Status != OrderStatus.Pending)
                {
                    _logger.LogWarning("Order with ID {OrderId} is not in a payable state: {Status}", request.OrderId, order.Status);
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 400,
                        Message = "Order is not in a payable state",
                        Data = null
                    };
                }

                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault();
                if (payment == null)
                {
                    _logger.LogWarning("Payment record not found for order ID: {OrderId}", request.OrderId);
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 404,
                        Message = "Payment record not found",
                        Data = null
                    };
                }

                if (payment.Amount < 0)
                {
                    _logger.LogWarning("Payment amount cannot be negative for order ID: {OrderId}", request.OrderId);
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 400,
                        Message = "Payment amount cannot be negative.",
                        Data = null
                    };
                }

                if (payment.PaymentMethod == PaymentMethod.CashOnDelivery)
                {
                    // COD không cần thanh toán trước
                    payment.PaymentStatus = PaymentStatus.Pending;
                    order.Status = OrderStatus.Confirmed;
                    _logger.LogInformation("Processed COD payment for order ID: {OrderId}", order.Id);
                }
                else
                {
                    // Thanh toán online
                    var paymentSuccess = await _paymentProcessor.ProcessPaymentAsync(payment);
                    if (paymentSuccess)
                    {
                        payment.PaymentStatus = PaymentStatus.Received;
                        order.AmountPaid = payment.Amount;
                        order.Status = OrderStatus.Confirmed;
                        _logger.LogInformation("Successfully processed online payment for order ID: {OrderId}", order.Id);
                    }
                    else
                    {
                        payment.PaymentStatus = PaymentStatus.Rejected;
                        order.Status = OrderStatus.Failed;
                        _logger.LogWarning("Failed to process online payment for order ID: {OrderId}", order.Id);
                    }
                }

                payment.UpdatedAt = DateTime.UtcNow;
                order.UpdatedAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);
                await _orderRepository.UpdateAsync(order);

                return new BaseResponseModel<ProcessPaymentResponse>
                {
                    Code = 200,
                    Message = "Payment processed successfully",
                    Data = new ProcessPaymentResponse
                    {
                        PaymentId = payment.Id,
                        Status = payment.PaymentStatus
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order ID: {OrderId}", request.OrderId);
                return HandleException<ProcessPaymentResponse>(ex, "processing payment");
            }
        }

        public async Task<BaseResponseModel<UpdateOrderStatusResponse>> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            try
            {
                _logger.LogInformation("Updating status for order ID: {OrderId}", request.OrderId);

                var order = await _orderRepository.FindAsync(request.OrderId);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", request.OrderId);
                    return new BaseResponseModel<UpdateOrderStatusResponse>
                    {
                        Code = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }

                // Kiểm tra trạng thái hợp lệ
                if (!IsValidOrderStatusTransition(order.Status, request.Status) ||
                    !IsValidShippingStatusTransition(order.ShippingStatus, request.ShippingStatus))
                {
                    _logger.LogWarning("Invalid status transition for order ID: {OrderId}. Current: {CurrentStatus}/{CurrentShippingStatus}, Requested: {RequestedStatus}/{RequestedShippingStatus}",
                        request.OrderId, order.Status, order.ShippingStatus, request.Status, request.ShippingStatus);
                    return new BaseResponseModel<UpdateOrderStatusResponse>
                    {
                        Code = 400,
                        Message = "Invalid status transition",
                        Data = null
                    };
                }

                // Kiểm tra thêm khi chuyển sang trạng thái Completed
                if (request.Status == OrderStatus.Completed)
                {
                    var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                    var payment = payments.FirstOrDefault();
                    if (payment == null || (payment.PaymentMethod != PaymentMethod.CashOnDelivery && payment.PaymentStatus != PaymentStatus.Received))
                    {
                        _logger.LogWarning("Cannot complete order ID: {OrderId}. Payment not received.", request.OrderId);
                        return new BaseResponseModel<UpdateOrderStatusResponse>
                        {
                            Code = 400,
                            Message = "Cannot complete order: Payment not received.",
                            Data = null
                        };
                    }
                }

                // Xử lý các trường hợp đặc biệt
                if (request.Status == OrderStatus.Processing && order.Status == OrderStatus.Confirmed)
                {
                    order.ShippingStatus = OrderShippingStatus.Shipping;
                }
                else if (request.Status == OrderStatus.Completed && order.Status == OrderStatus.Processing)
                {
                    order.ShippingStatus = OrderShippingStatus.Delivered;
                    var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                    var payment = payments.FirstOrDefault();
                    if (payment != null && payment.PaymentMethod == PaymentMethod.CashOnDelivery)
                    {
                        payment.PaymentStatus = PaymentStatus.Received;
                        order.AmountPaid = payment.Amount;
                        payment.UpdatedAt = DateTime.UtcNow;
                        await _paymentRepository.UpdateAsync(payment);
                        _logger.LogInformation("Processed COD payment on completion for order ID: {OrderId}", order.Id);
                    }
                }
                else if (request.Status == OrderStatus.Refunded || request.Status == OrderStatus.Cancelled)
                {
                    var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                    var payment = payments.FirstOrDefault();
                    if (payment != null && payment.PaymentStatus == PaymentStatus.Received)
                    {
                        payment.PaymentStatus = PaymentStatus.Rejected; // Giả lập hoàn tiền
                        payment.UpdatedAt = DateTime.UtcNow;
                        await _paymentRepository.UpdateAsync(payment);
                        _logger.LogInformation("Refunded payment for order ID: {OrderId}", order.Id);
                    }
                }

                order.Status = request.Status;
                order.ShippingStatus = request.ShippingStatus;
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateAsync(order);
                _logger.LogInformation("Successfully updated status for order ID: {OrderId} to {Status}/{ShippingStatus}", order.Id, order.Status, order.ShippingStatus);

                return new BaseResponseModel<UpdateOrderStatusResponse>
                {
                    Code = 200,
                    Message = "Order status updated successfully",
                    Data = new UpdateOrderStatusResponse
                    {
                        OrderId = order.Id,
                        Status = order.Status,
                        ShippingStatus = order.ShippingStatus
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order ID: {OrderId}", request.OrderId);
                return HandleException<UpdateOrderStatusResponse>(ex, "updating order status");
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<GetOrderByUserIdResponse>> GetOrdersByUserIdAsync(GetOrdersByUserIdRequest request)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId);
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException($"No orders found for user with ID {request.UserId}");
            }

            return orders.Select(o => new GetOrderByUserIdResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                ShippingInfoId = o.ShippingInfoId,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                Note = null,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                OrderDetails = o.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    SizeId = od.SizeId
                }).ToList() ?? new List<OrderDetailDto>()
            }).ToList();
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            return new GetOrderByIdResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                ShippingInfoId = order.ShippingInfoId,
                VoucherId = order.VoucherId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Note = null,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    SizeId = od.SizeId
                }).ToList() ?? new List<OrderDetailDto>()
            };
        }

        private async Task<List<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found");
            }

            return orders.Select(o => new GetOrderByIdResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                ShippingInfoId = o.ShippingInfoId,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                Note = null,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                OrderDetails = o.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    SizeId = od.SizeId
                }).ToList() ?? new List<OrderDetailDto>()
            }).ToList();
        }

        private async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            order.ShippingInfoId = request.ShippingInfoId;
            order.VoucherId = request.VoucherId;
            order.TotalPrice = request.TotalPrice;
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            var existingOrderDetails = await _orderDetailRepository.GetByOrderIdAsync(order.Id);
            foreach (var existingDetail in existingOrderDetails)
            {
                await _orderDetailRepository.DeleteAsync(existingDetail.Id);
            }

            foreach (var od in request.OrderDetails)
            {
                var newOrderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    SizeId = od.SizeId,
                    Quantity = od.Quantity,
                    Price = od.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _orderDetailRepository.AddAsync(newOrderDetail);
            }

            await _orderRepository.UpdateAsync(order);

            return new UpdateOrderResponse
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status
            };
        }

        private float CalculateOrderDetailsTotal(List<OrderDetailDto> orderDetails)
        {
            return orderDetails.Sum(od => od.Quantity * od.UnitPrice);
        }

        private (bool isValid, string message) IsVoucherValid(Voucher voucher)
        {
            // Kiểm tra trạng thái
            if (voucher.Status != 0) // Giả định 0 là Active
            {
                return (false, "Voucher is not active.");
            }

            // Kiểm tra ngày hết hạn
            if (voucher.ExpirationDate < DateTime.UtcNow)
            {
                return (false, "Voucher has expired.");
            }

            // Kiểm tra VoucherValue
            if (string.IsNullOrWhiteSpace(voucher.VoucherValue) || !float.TryParse(voucher.VoucherValue, out float discount) || discount <= 0)
            {
                return (false, "Invalid voucher value.");
            }

            return (true, string.Empty);
        }

        private float ApplyVoucherDiscount(float totalPrice, Voucher voucher)
        {
            if (float.TryParse(voucher.VoucherValue, out float discount))
            {
                return Math.Max(0, totalPrice - discount);
            }
            return totalPrice; // Nếu không parse được, không áp dụng giảm giá
        }

        private bool IsValidOrderStatusTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Pending, OrderStatus.Failed) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Completed) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,
                (OrderStatus.Failed, OrderStatus.Pending) => true,
                _ => false
            };
        }

        private bool IsValidShippingStatusTransition(OrderShippingStatus current, OrderShippingStatus next)
        {
            return (current, next) switch
            {
                (OrderShippingStatus.Preparing, OrderShippingStatus.Shipping) => true,
                (OrderShippingStatus.Preparing, OrderShippingStatus.Cancelled) => true,
                (OrderShippingStatus.Shipping, OrderShippingStatus.Delivered) => true,
                (OrderShippingStatus.Shipping, OrderShippingStatus.Returned) => true,
                (OrderShippingStatus.Shipping, OrderShippingStatus.Lost) => true,
                _ => false
            };
        }

        private BaseResponseModel<T> HandleException<T>(Exception ex, string action) =>
            new BaseResponseModel<T>
            {
                Code = 500,
                Message = $"Error {action}: {ex.Message}",
                Data = default
            };

        #endregion
    }
}