using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.PaymentService;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
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
                return new BaseResponseModel<List<GetOrderByUserIdResponse>>
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
                return new BaseResponseModel<List<GetOrderByIdResponse>>
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
                var user = await _userRepository.GetUserNameByUserIdAsync(order.UserId);
                var voucher = await _voucherRepository.FindAsync(order.VoucherId);              
                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault();

                result.Add(new GetOrderByUserIdResponse(
                    status: order.Status,
                    shippingStatus: order.ShippingStatus,
                    paymentMethod: payment?.PaymentMethod ?? PaymentMethod.Wallet
                )
                {
                    Id = order.Id,
                    UserName = user?.Name,
                    ShippingInfoId = order.ShippingInfoId,
                    VoucherCode = voucher?.VoucherName,
                    TotalPrice = order.TotalPrice,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                    {
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        SizeValue = od.Size.SizeValue,
                    }).ToList() ?? new List<OrderDetailResponseDto>()
                });
            }

            return result;
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.GetAll()
                                              .Include(o => o.OrderDetails)
                                              .FirstOrDefaultAsync(o => o.Id == request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }
            var user = await _userRepository.GetUserNameByUserIdAsync(order.UserId);
            var voucher = await _voucherRepository.FindAsync(order.VoucherId);
            var size = await _sizeRepository.FindAsync(order.OrderDetails.FirstOrDefault().SizeId);
            var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
            var payment = payments.FirstOrDefault();

            return new GetOrderByIdResponse
            (
            status: order.Status,
            shippingStatus: order.ShippingStatus,
            paymentMethod: payment?.PaymentMethod ?? PaymentMethod.Wallet
             )
            {
                Id = order.Id,               
                UserName = user?.Name,
                ShippingInfoId = order.ShippingInfoId,          
                VoucherCode = voucher?.VoucherName,
                TotalPrice = order.TotalPrice,            
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    SizeValue = size.SizeValue,
                }).ToList() ?? new List<OrderDetailResponseDto>()
            };
        }

        private async Task<List<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll()
                                                .Include(o => o.OrderDetails)
                                                .ThenInclude(od => od.Size)
                                                .ToListAsync();


            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found");
            }

            var result = new List<GetOrderByIdResponse>();
            foreach (var order in orders)
            {
                var user = await _userRepository.GetUserNameByUserIdAsync(order.UserId);
                var voucher = await _voucherRepository.FindAsync(order.VoucherId);
                var payments = await _paymentRepository.GetByOrderIdAsync(order.Id);
                var payment = payments.FirstOrDefault();

                result.Add(new GetOrderByIdResponse(
                               status: order.Status,
                               shippingStatus: order.ShippingStatus,
                               paymentMethod: payment?.PaymentMethod ?? PaymentMethod.Wallet
                                                    )
                     {
                    Id = order.Id,                
                    UserName = user?.Name,
                    ShippingInfoId = order.ShippingInfoId,                    
                    VoucherCode = voucher?.VoucherName,
                    TotalPrice = order.TotalPrice,                
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDto
                    {
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        UnitPrice = od.Price,
                        SizeValue = od.Size.SizeValue,
                    }).ToList() ?? new List<OrderDetailResponseDto>()
                });
            }

            return result;
        }

        private async Task<Order> GetEntityFromAddOrderRequest(AddOrderRequest request)
        {
            float totalAmount = 0;
            foreach (var od in request.OrderDetails)
            {
                var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);
                totalAmount += od.Quantity * customShoeDesign.TotalAmount;
            }

            if (totalAmount <= 0)
            {
                throw new InvalidOperationException("Order total must be greater than 0.");
            }

            float amountPaid = totalAmount;
            if (request.VoucherId.HasValue)
            {
                var voucher = await _voucherRepository.GetVoucherByOrderIdAsync(request.VoucherId.Value);
                if (voucher == null)
                {
                    throw new InvalidOperationException("Voucher not found");
                }else if(IsVoucherValid(voucher).isValid == false)
                {
                    throw new InvalidOperationException(IsVoucherValid(voucher).message);
                }
                var discountAmount = float.TryParse(voucher.VoucherValue, out float discountValue) ? discountValue : 0;
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
                AmountPaid = amountPaid + 30000,
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
                var customShoeDesign = await _customShoeDesignRepository.FindAsync(od.CustomShoeDesignId);

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
                    Price = customShoeDesign.TotalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }
        }

        private async Task<string> AddPaymentAsync(Order order, PaymentMethod paymentMethod)
        {
            var payment = new DTOs.Payment.AddPaymentRequest
            {
                OrderId = order.Id,
                Amount = (int)order.TotalPrice,
                PaymentMethod = paymentMethod,
            };
            var paymentResponse = await _paymentService.AddPayment(payment);
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
        #endregion
    }
}