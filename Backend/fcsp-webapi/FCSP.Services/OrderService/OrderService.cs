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
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IPaymentProcessor _paymentProcessor;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IPaymentRepository paymentRepository,
            IVoucherRepository voucherRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IPaymentProcessor paymentProcessor)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _paymentRepository = paymentRepository;
            _voucherRepository = voucherRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _paymentProcessor = paymentProcessor;
        }

        #region Public Methods

        public async Task<BaseResponseModel<List<GetOrderByUserIdResponse>>> GetOrdersByUserId(GetOrdersByUserIdRequest request)
        {
            try
            {
                var orders = await GetOrdersByUserIdAsync(request);
                return new BaseResponseModel<List<GetOrderByUserIdResponse>>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<GetOrderByUserIdResponse>>(ex, "retrieving orders by user ID");
            }
        }

        public async Task<BaseResponseModel<GetOrderByIdResponse>> GetOrderById(GetOrderByIdRequest request)
        {
            try
            {
                var order = await GetOrderByIdAsync(request);
                return new BaseResponseModel<GetOrderByIdResponse>
                {
                    Code = 200,
                    Message = "Order retrieved successfully",
                    Data = order
                };
            }
            catch (Exception ex)
            {
                return HandleException<GetOrderByIdResponse>(ex, "retrieving order by ID");
            }
        }

        public async Task<BaseResponseModel<List<GetOrderByIdResponse>>> GetAllOrders()
        {
            try
            {
                var orders = await GetAllOrdersAsync();
                return new BaseResponseModel<List<GetOrderByIdResponse>>
                {
                    Code = 200,
                    Message = "All orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<GetOrderByIdResponse>>(ex, "retrieving all orders");
            }
        }

        public async Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request)
        {
            try
            {
                float calculatedTotal = 0;
                foreach (var od in request.OrderDetails) // OrderDetails giờ là List<OrderDetailRequestDto>
                {
                    var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                    if (customShoeDesign == null)
                    {
                        return new BaseResponseModel<AddOrderResponse>
                        {
                            Code = 400,
                            Message = $"CustomShoeDesign with ID {od.CustomShoeDesignId} not found.",
                            Data = null
                        };
                    }

                    calculatedTotal += od.Quantity * customShoeDesign.TotalAmount;
                }

                if (calculatedTotal <= 0)
                {
                    return new BaseResponseModel<AddOrderResponse>
                    {
                        Code = 400,
                        Message = "Order total must be greater than 0.",
                        Data = null
                    };
                }

                float totalPrice = calculatedTotal;
                if (request.VoucherId.HasValue)
                {
                    var voucher = await _voucherRepository.FindByIdAsync(request.VoucherId.Value);
                    if (voucher == null)
                    {
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
                        return new BaseResponseModel<AddOrderResponse>
                        {
                            Code = 400,
                            Message = $"Invalid voucher: {validationMessage}.",
                            Data = null
                        };
                    }

                    totalPrice = ApplyVoucherDiscount(totalPrice, voucher);
                }

                if (totalPrice <= 0)
                {
                    return new BaseResponseModel<AddOrderResponse>
                    {
                        Code = 400,
                        Message = "Final order total must be greater than 0 after applying voucher.",
                        Data = null
                    };
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

                // Tạo OrderDetails
                foreach (var od in request.OrderDetails) // OrderDetails là List<OrderDetailRequestDto>
                {
                    var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                    var orderDetail = new OrderDetail
                    {
                        OrderId = addedOrder.Id,
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        SizeId = od.SizeId,
                        Quantity = od.Quantity,
                        Price = customShoeDesign.TotalAmount,
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

                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 201,
                    Message = "Order added successfully",
                    Data = new AddOrderResponse
                    {
                        Id = addedOrder.Id,
                        TotalPrice = addedOrder.TotalPrice,
                        Status = addedOrder.Status,
                        ShippingStatus = addedOrder.ShippingStatus
                    }
                };
            }
            catch (Exception ex)
            {
                return HandleException<AddOrderResponse>(ex, "adding order");
            }
        }

        public async Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                // Lấy TotalAmount từ CustomShoeDesign và tính tổng
                float calculatedTotal = 0;
                foreach (var od in request.OrderDetails) // OrderDetails là List<OrderDetailRequestDto>
                {
                    var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                    if (customShoeDesign == null)
                    {
                        return new BaseResponseModel<UpdateOrderResponse>
                        {
                            Code = 400,
                            Message = $"CustomShoeDesign with ID {od.CustomShoeDesignId} not found.",
                            Data = null
                        };
                    }

                    if (customShoeDesign.TotalAmount <= 0)
                    {
                        return new BaseResponseModel<UpdateOrderResponse>
                        {
                            Code = 400,
                            Message = $"CustomShoeDesign with ID {od.CustomShoeDesignId} has invalid total amount. Total amount must be greater than 0.",
                            Data = null
                        };
                    }

                    calculatedTotal += od.Quantity * customShoeDesign.TotalAmount;
                }

                if (calculatedTotal <= 0)
                {
                    return new BaseResponseModel<UpdateOrderResponse>
                    {
                        Code = 400,
                        Message = "Order total must be greater than 0.",
                        Data = null
                    };
                }

                // Validate Voucher (nếu có) và áp dụng giảm giá
                float totalPrice = calculatedTotal;
                if (request.VoucherId.HasValue)
                {
                    var voucher = await _voucherRepository.FindByIdAsync(request.VoucherId.Value);
                    if (voucher == null)
                    {
                        return new BaseResponseModel<UpdateOrderResponse>
                        {
                            Code = 400,
                            Message = "Voucher not found.",
                            Data = null
                        };
                    }

                    var (isValid, validationMessage) = IsVoucherValid(voucher);
                    if (!isValid)
                    {
                        return new BaseResponseModel<UpdateOrderResponse>
                        {
                            Code = 400,
                            Message = $"Invalid voucher: {validationMessage}.",
                            Data = null
                        };
                    }

                    totalPrice = ApplyVoucherDiscount(totalPrice, voucher);
                }

                // Kiểm tra totalPrice sau khi áp dụng voucher
                if (totalPrice <= 0)
                {
                    return new BaseResponseModel<UpdateOrderResponse>
                    {
                        Code = 400,
                        Message = "Final order total must be greater than 0 after applying voucher.",
                        Data = null
                    };
                }

                var updatedOrder = await UpdateOrderAsync(request, totalPrice);
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 200,
                    Message = "Order updated successfully",
                    Data = updatedOrder
                };
            }
            catch (Exception ex)
            {
                return HandleException<UpdateOrderResponse>(ex, "updating order");
            }
        }

        public async Task<BaseResponseModel<DeleteOrderResponse>> DeleteOrder(DeleteOrderRequest request)
        {
            try
            {
                var order = await _orderRepository.FindAsync(request.Id);
                if (order == null)
                {
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
                }
                else
                {
                    order.Status = OrderStatus.Cancelled;
                }

                order.ShippingStatus = OrderShippingStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);

                return new BaseResponseModel<DeleteOrderResponse>
                {
                    Code = 200,
                    Message = "Order cancelled successfully",
                    Data = new DeleteOrderResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return HandleException<DeleteOrderResponse>(ex, "deleting order");
            }
        }

        public async Task<BaseResponseModel<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request)
        {
            try
            {
                var order = await _orderRepository.FindAsync(request.OrderId);
                if (order == null)
                {
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 404,
                        Message = "Order not found",
                        Data = null
                    };
                }

                if (order.Status != OrderStatus.Pending)
                {
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
                    return new BaseResponseModel<ProcessPaymentResponse>
                    {
                        Code = 404,
                        Message = "Payment record not found",
                        Data = null
                    };
                }

                if (payment.Amount < 0)
                {
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
                    }
                    else
                    {
                        payment.PaymentStatus = PaymentStatus.Rejected;
                        order.Status = OrderStatus.Failed;
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
                return HandleException<ProcessPaymentResponse>(ex, "processing payment");
            }
        }

        public async Task<BaseResponseModel<UpdateOrderStatusResponse>> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _orderRepository.FindAsync(request.OrderId);
                if (order == null)
                {
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
                    }
                }

                order.Status = request.Status;
                order.ShippingStatus = request.ShippingStatus;
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateAsync(order);

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

            var result = new List<GetOrderByUserIdResponse>();
            foreach (var order in orders)
            {
                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault();

                result.Add(new GetOrderByUserIdResponse
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    ShippingInfoId = order.ShippingInfoId,
                    VoucherId = order.VoucherId,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    ShippingStatus = order.ShippingStatus,
                    PaymentMethod = payment?.PaymentMethod ?? PaymentMethod.CashOnDelivery,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                    {
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        SizeId = od.SizeId
                    }).ToList() ?? new List<OrderDetailResponseDto>()
                });
            }

            return result;
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
            var payment = payments.FirstOrDefault();

            return new GetOrderByIdResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                ShippingInfoId = order.ShippingInfoId,
                VoucherId = order.VoucherId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                ShippingStatus = order.ShippingStatus,
                PaymentMethod = payment?.PaymentMethod ?? PaymentMethod.CashOnDelivery,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    SizeId = od.SizeId
                }).ToList() ?? new List<OrderDetailResponseDto>()
            };
        }

        private async Task<List<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found");
            }

            var result = new List<GetOrderByIdResponse>();
            foreach (var order in orders)
            {
                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault();

                result.Add(new GetOrderByIdResponse
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    ShippingInfoId = order.ShippingInfoId,
                    VoucherId = order.VoucherId,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    ShippingStatus = order.ShippingStatus,
                    PaymentMethod = payment?.PaymentMethod ?? PaymentMethod.CashOnDelivery,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                    {
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        SizeId = od.SizeId
                    }).ToList() ?? new List<OrderDetailResponseDto>()
                });
            }

            return result;
        }

        private async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest request, float totalPrice)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            order.ShippingInfoId = request.ShippingInfoId;
            order.VoucherId = request.VoucherId;
            order.TotalPrice = totalPrice;
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            var existingOrderDetails = await _orderDetailRepository.GetByOrderIdAsync(order.Id);
            foreach (var existingDetail in existingOrderDetails)
            {
                await _orderDetailRepository.DeleteAsync(existingDetail.Id);
            }

            foreach (var od in request.OrderDetails) // OrderDetails là List<OrderDetailRequestDto>
            {
                var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                var newOrderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    SizeId = od.SizeId,
                    Quantity = od.Quantity,
                    Price = customShoeDesign.TotalAmount,
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
                Status = order.Status,
                ShippingStatus = order.ShippingStatus
            };
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
            if (float.TryParse(voucher.VoucherValue, out float discount) && discount > 0)
            {
                return Math.Max(0, totalPrice - discount);
            }
            return totalPrice;
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