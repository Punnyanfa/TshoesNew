using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.PaymentService;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISizeRepository _sizeRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IPaymentService paymentService,
            IPaymentRepository paymentRepository,
            IVoucherRepository voucherRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IUserRepository userRepository,
            ISizeRepository sizeRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
            _voucherRepository = voucherRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _userRepository = userRepository;
            _sizeRepository = sizeRepository;
        }

        #region Public Methods
        public async Task<BaseResponseModel<IEnumerable<GetOrderByIdResponse>>> GetOrdersByUserId(GetOrdersByUserIdRequest request)
        {
            try
            {
                var orders = await GetOrdersByUserIdAsync(request);
                return new BaseResponseModel<IEnumerable<GetOrderByIdResponse>>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<GetOrderByIdResponse>>
                {
                    Code = 500,
                    Message = $"Error retrieving orders by user ID: {ex.Message}",
                    Data = null
                };
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
                return new BaseResponseModel<GetOrderByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving order by ID: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<IEnumerable<GetOrderByIdResponse>>> GetAllOrders()
        {
            try
            {
                var orders = await GetAllOrdersAsync();
                return new BaseResponseModel<IEnumerable<GetOrderByIdResponse>>
                {
                    Code = 200,
                    Message = "All orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<GetOrderByIdResponse>>
                {
                    Code = 500,
                    Message = $"Error retrieving all orders: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request)
        {
            try
            {
                var order = await GetEntityFromAddOrderRequest(request);
                var addedOrder = await _orderRepository.AddAsync(order);
                await AddOrderDetailsAsync(addedOrder, request.OrderDetails);
                var paymentUrl = await AddPaymentAsync(addedOrder, request.PaymentMethod);

                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 200,
                    Message = "Order added successfully",
                    Data = new AddOrderResponse
                    {
                        PaymentUrl = paymentUrl
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 500,
                    Message = $"Error adding order: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                var order = await GetEntityFromUpdateOrderRequest(request);
                await _orderRepository.UpdateAsync(order);

                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 200,
                    Message = "Order updated successfully",
                    Data = new UpdateOrderResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 500,
                    Message = $"Error updating order: {ex.Message}",
                    Data = new UpdateOrderResponse
                    {
                        Success = false
                    }
                };
            }
        }
        #endregion

        #region Private Methods

        private async Task<IEnumerable<GetOrderByIdResponse>> GetOrdersByUserIdAsync(GetOrdersByUserIdRequest request)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId);
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException($"No orders found for user with ID {request.UserId}");
            }

            return orders.Select(o => new GetOrderByIdResponse
            {
                Id = o.Id,
                UserName = o.User?.Name ?? string.Empty,
                ShippingInfoId = o.ShippingInfoId,
                VoucherCode = o.Voucher?.VoucherName ?? string.Empty,
                Status = o.Status.ToString(),
                ShippingStatus = o.ShippingStatus.ToString(),
                PaymentMethod = o.Payments?.FirstOrDefault()?.PaymentMethod.ToString() ?? string.Empty,
                TotalPrice = o.TotalPrice,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            }
            );
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.Id);

            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            return new GetOrderByIdResponse
            {
                Id = order.Id,
                UserName = order.User?.Name ?? string.Empty,
                ShippingInfoId = order.ShippingInfoId,
                VoucherCode = order.Voucher?.VoucherName ?? string.Empty,
                Status = order.Status.ToString(),
                ShippingStatus = order.ShippingStatus.ToString(),
                PaymentMethod = order.Payments?.FirstOrDefault()?.PaymentMethod.ToString() ?? string.Empty,
                TotalPrice = order.TotalPrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.TotalPrice,
                    TemplatePrice = od.TemplatePrice,
                    ServicePrice = od.ServicePrice,
                    DesignerMarkup = od.DesignerMarkup,
                    SizeValue = od.Size.SizeValue
                }).ToList() ?? new List<OrderDetailResponseDto>()
            };
        }

        private async Task<IEnumerable<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll()
                                               .Include(o => o.OrderDetails)
                                               .ThenInclude(od => od.Size)
                                               .Include(o => o.User)
                                               .Include(o => o.Voucher)
                                               .Include(o => o.Payments)
                                               .ToListAsync();

            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found");
            }

            return orders.Select(o => new GetOrderByIdResponse
            {
                Id = o.Id,
                UserName = o.User?.Name ?? string.Empty,
                ShippingInfoId = o.ShippingInfoId,
                VoucherCode = o.Voucher?.VoucherName ?? string.Empty,
                Status = o.Status.ToString(),
                ShippingStatus = o.ShippingStatus.ToString(),
                PaymentMethod = o.Payments?.FirstOrDefault()?.PaymentMethod.ToString() ?? string.Empty,
                TotalPrice = o.TotalPrice,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            });
        }

        private async Task<Order> GetEntityFromAddOrderRequest(AddOrderRequest request)
        {
            var totalAmount = 0;
            foreach (var od in request.OrderDetails)
            {
                var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                totalAmount += od.Quantity * customShoeDesign.TotalAmount;
            }

            if (totalAmount <= 0)
            {
                throw new InvalidOperationException("Order total must be greater than 0.");
            }

            var amountPaid = totalAmount;
            if (request.VoucherId.HasValue)
            {
                var voucher = await _voucherRepository.GetVoucherByOrderIdAsync(request.VoucherId.Value);
                if (voucher == null)
                {
                    throw new InvalidOperationException("Voucher not found");
                }
                else if (IsVoucherValid(voucher).isValid == false)
                {
                    throw new InvalidOperationException(IsVoucherValid(voucher).message);
                }
                var discountAmount = int.TryParse(voucher.VoucherValue, out int discountValue) ? discountValue : 0;
                amountPaid = amountPaid - discountAmount;
            }

            if (amountPaid <= 0)
            {
                throw new InvalidOperationException("Order total must be greater than 0.");
            }

            var order = new Order
            {
                UserId = request.UserId,
                ShippingInfoId = request.ShippingInfoId,
                VoucherId = request.VoucherId ?? null,
                TotalPrice = totalAmount,
                AmountPaid = amountPaid,
                Status = OrderStatus.Pending,
                ShippingStatus = OrderShippingStatus.Preparing,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return order;
        }

        private async Task AddOrderDetailsAsync(Order order, List<OrderDetailRequestDto> orderDetails)
        {
            foreach (var od in orderDetails)
            {
                var customShoeDesign = await _customShoeDesignRepository.GetAll()
                                                                        .FirstOrDefaultAsync(csd => csd.Id == od.CustomShoeDesignId 
                                                                                        && csd.IsDeleted == false 
                                                                                        && (csd.Status == CustomShoeDesignStatus.Public 
                                                                                        || csd.Status == CustomShoeDesignStatus.Private));
                if (customShoeDesign == null)
                {
                    throw new InvalidOperationException($"CustomShoeDesign with ID {od.CustomShoeDesignId} not found.");
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    SizeId = od.SizeId,
                    Quantity = od.Quantity,
                    TotalPrice = customShoeDesign.TotalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ManufacturerId = od.ManufacturerId
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }
        }

        private async Task<string> AddPaymentAsync(Order order, PaymentMethod paymentMethod)
        {
            var payment = new AddPaymentRequest
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                PaymentMethod = paymentMethod,
            };
            
            var paymentResponse = await _paymentService.AddPayment(payment);

            if (paymentResponse == null || paymentResponse.Data == null)
            {
                throw new InvalidOperationException("Payment failed");
            }

            return paymentResponse.Data.Response;
        }

        private async Task<Order> GetEntityFromUpdateOrderRequest(UpdateOrderRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            return order;
        }

        private (bool isValid, string message) IsVoucherValid(Voucher voucher)
        {
            if (voucher.Status != 0)
            {
                return (false, "Voucher is not active.");
            }

            if (voucher.ExpirationDate < DateTime.UtcNow)
            {
                return (false, "Voucher has expired.");
            }

            if (string.IsNullOrWhiteSpace(voucher.VoucherValue) || !int.TryParse(voucher.VoucherValue, out int discount) || discount <= 0)
            {
                return (false, "Invalid voucher value.");
            }

            return (true, string.Empty);
        }

        private bool IsValidOrderStatusTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Completed) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,
                _ => false
            };
        }

        private bool IsValidShippingStatusTransition(OrderShippingStatus current, OrderShippingStatus next)
        {
            return (current, next) switch
            {
                (OrderShippingStatus.Preparing, OrderShippingStatus.Shipping) => true,
                (OrderShippingStatus.Shipping, OrderShippingStatus.Delivered) => true,
                (OrderShippingStatus.Shipping, OrderShippingStatus.Returned) => true,
                _ => false
            };
        }
        #endregion
    }
}