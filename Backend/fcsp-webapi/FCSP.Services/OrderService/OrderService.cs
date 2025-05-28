using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.DTOs.OrderDetail;
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
        private readonly IShippingInfoRepository _shippingInfoRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly ICustomShoeDesignTemplateRepository _customShoeDesignTemplateRepository;
        

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IPaymentService paymentService,
            IPaymentRepository paymentRepository,
            IVoucherRepository voucherRepository,
            IShippingInfoRepository shippingInfoRepository,
            ICustomShoeDesignRepository customShoeDesignRepository,
            IUserRepository userRepository,
            ISizeRepository sizeRepository,
            ICustomShoeDesignTemplateRepository customShoeDesignTemplateRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
            _voucherRepository = voucherRepository;
            _shippingInfoRepository = shippingInfoRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _userRepository = userRepository;
            _sizeRepository = sizeRepository;
            _customShoeDesignTemplateRepository = customShoeDesignTemplateRepository;
        }

        #region Public Methods
        public async Task<BaseResponseModel<GetOrdersByUserIdResponse>> GetOrdersByUserId(GetOrdersByUserIdRequest request)
        {
            try
            {
                var orders = await GetOrdersByUserIdAsync(request);
                return new BaseResponseModel<GetOrdersByUserIdResponse>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = new GetOrdersByUserIdResponse
                    {
                        Orders = orders
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetOrdersByUserIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving orders by user ID: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetOrderByManufacturerIdResponse>> GetOrdersByManufacturerId(GetOrderByManufacturerIdRequest request)
        {
            try
            {
                var order = await GetOrdersByManufacturerIdAsync(request);
                return new BaseResponseModel<GetOrderByManufacturerIdResponse>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = new GetOrderByManufacturerIdResponse
                    {
                        Orders = order
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetOrderByManufacturerIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving orders by manufacturer ID: {ex.Message}",
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
                await AddOrderDetailsAsync(addedOrder, request.OrderDetail);
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
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = ex.Message.Contains("not found") ? 404 : 400,
                    Message = ex.Message,
                    Data = null
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
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = ex.Message.Contains("not found") ? 404 : 400,
                    Message = ex.Message,
                    Data = new UpdateOrderResponse
                    {
                        Success = false
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
            var orders = await _orderRepository.GetAll()
                                                .Include(o => o.OrderDetails)
                                                    .ThenInclude(od => od.Size)
                                                .Include(o => o.OrderDetails)
                                                    .ThenInclude(od => od.CustomShoeDesign)
                                                        .ThenInclude(cd => cd.DesignPreviews)
                                                .Include(o => o.User)
                                                .Include(o => o.Voucher)
                                                .Include(o => o.Payments)
                                                .Where(o => o.UserId == request.UserId)
                                                .ToListAsync();

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
                UpdatedAt = o.UpdatedAt,
                OrderDetail = new OrderDetailResponseDto
                {
                    CustomShoeDesignName = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.Name,
                    CustomShoeDesignDescription = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.Description,
                    FirstPreviewImageUrl = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
                    Quantity = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.Quantity ?? 0,
                    UnitPrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.TotalPrice ?? 0,
                    TemplatePrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.TemplatePrice ?? 0,
                    ServicePrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.ServicePrice ?? 0,
                    DesignerMarkup = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.DesignerMarkup ?? 0,
                    SizeValue = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.Size.SizeValue ?? 0
                }
            });
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.Id);

            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            var orderDetail = await _orderDetailRepository.GetByOrderIdAsync(request.Id);

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
                OrderDetail = new OrderDetailResponseDto
                {
                    CustomShoeDesignName = orderDetail.CustomShoeDesign?.Name,
                    CustomShoeDesignDescription = orderDetail.CustomShoeDesign?.Description,
                    FirstPreviewImageUrl = orderDetail.CustomShoeDesign?.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.TotalPrice,
                    TemplatePrice = orderDetail.TemplatePrice,
                    ServicePrice = orderDetail.ServicePrice,
                    DesignerMarkup = orderDetail.DesignerMarkup,
                    SizeValue = orderDetail.Size.SizeValue
                }
            };
        }

        private async Task<IEnumerable<GetOrderByIdResponse>> GetOrdersByManufacturerIdAsync(GetOrderByManufacturerIdRequest request)
        {
            var orders = await _orderRepository.GetAll()
                                                .Include(o => o.OrderDetails)
                                                .ThenInclude(od => od.Size)
                                                .Include(o => o.User)
                                                .Include(o => o.Voucher)
                                                .Include(o => o.Payments)
                                                .Where(o => o.OrderDetails.Any(o => o.ManufacturerId == request.ManufacturerId))
                                                .ToListAsync();
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException($"No orders found for manufacturer with ID {request.ManufacturerId}");
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

        private async Task<IEnumerable<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll()
                                               .Include(o => o.OrderDetails)
                                                   .ThenInclude(od => od.Size)
                                               .Include(o => o.OrderDetails)
                                                   .ThenInclude(od => od.CustomShoeDesign)
                                                       .ThenInclude(cd => cd.DesignPreviews)
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
                UpdatedAt = o.UpdatedAt,
                OrderDetail = new OrderDetailResponseDto
                {
                    CustomShoeDesignName = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.Name,
                    CustomShoeDesignDescription = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.Description,
                    FirstPreviewImageUrl = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.CustomShoeDesign?.DesignPreviews?.FirstOrDefault()?.PreviewImageUrl,
                    Quantity = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.Quantity ?? 0,
                    UnitPrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.TotalPrice ?? 0,
                    TemplatePrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.TemplatePrice ?? 0,
                    ServicePrice = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.ServicePrice ?? 0,
                    DesignerMarkup = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.DesignerMarkup ?? 0,
                    SizeValue = o.OrderDetails.FirstOrDefault(od => od.OrderId == o.Id)?.Size.SizeValue ?? 0
                }
            });
        }

        private async Task<Order> GetEntityFromAddOrderRequest(AddOrderRequest request)
        {
            // Validation for required fields
            if (request.UserId <= 0)
                throw new InvalidOperationException("userId can not be 0 ");
            if (request.ShippingInfoId <= 0)
                throw new InvalidOperationException("shippingInforId is require");

            if (request.OrderDetail == null)
                throw new InvalidOperationException("orderDetail is require");

            if (request.OrderDetail.CustomShoeDesignId <= 0)
                throw new InvalidOperationException("customeShoeDesignId is require");

            if (request.OrderDetail.Quantity <= 0)
                throw new InvalidOperationException("quantity is require");

            if (request.OrderDetail.Quantity > 1)
                throw new InvalidOperationException("Only 1 custom shoe design for 1 order");

            if (request.OrderDetail.SizeId <= 0)
                throw new InvalidOperationException("sizeId can not be 0");

            if (request.OrderDetail.ManufacturerId <= 0)
                throw new InvalidOperationException("manufacturerId is require");

            if (!Enum.IsDefined(typeof(PaymentMethod), request.PaymentMethod))
                throw new InvalidOperationException("paymentMethod is not valid");
                
            //Check if exsist
            var shippingInfor = await _shippingInfoRepository.FindAsync(request.ShippingInfoId);
            if(shippingInfor == null)
            {
                throw new InvalidOperationException($"Shipping Infor with Id {request.ShippingInfoId} not found");
            }
            var size = await _sizeRepository.FindAsync(request.OrderDetail.SizeId);
            if (size == null)
            {
                throw new InvalidOperationException($"Size with ID {request.OrderDetail.SizeId} not found");
            }          
            var user = await _userRepository.FindAsync(request.UserId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {request.UserId} not found");
                         

            var totalAmount = 0;

            var customShoeDesign = await _customShoeDesignRepository.GetAll()
                .Include(d => d.CustomShoeDesignTemplate)
                .Include(d => d.DesignServices)
                    .ThenInclude(ds => ds.Service)
                .FirstOrDefaultAsync(d => d.Id == request.OrderDetail.CustomShoeDesignId);

            

            var templatePrice = customShoeDesign.CustomShoeDesignTemplate?.Price ?? 0;
            var servicesPrice = customShoeDesign.DesignServices?.Sum(ds => ds.Service?.Price ?? 0) ?? 0;
            var designerMarkup = customShoeDesign.DesignerMarkup;

            totalAmount += request.OrderDetail.Quantity * (templatePrice + servicesPrice + designerMarkup);

            if (totalAmount <= 0)
            {
                throw new InvalidOperationException("Order total must be greater than 0.");
            }

            var amountPaid = totalAmount;
            if (request.VoucherId.HasValue)
            {
                var voucher = await _voucherRepository.FindAsync(request.VoucherId.Value);
                if (voucher == null)
                {
                    throw new InvalidOperationException($"VoucherId {request.VoucherId} not found");
                }
                else if (IsVoucherValid(voucher).isValid == false)
                {
                    throw new InvalidOperationException(IsVoucherValid(voucher).message);
                }
                var discountAmount = int.TryParse(voucher.VoucherValue, out int discountValue) ? discountValue : 0;
                amountPaid = amountPaid - discountAmount;
                voucher.Status = (int)VoucherStatus.Used;
                await _voucherRepository.UpdateAsync(voucher);   
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

        private async Task AddOrderDetailsAsync(Order order, OrderDetailRequestDto orderDetailToAdd)
        {
                var customShoeDesign = await _customShoeDesignRepository.GetAll()
                                                                        .FirstOrDefaultAsync(csd => csd.Id == orderDetailToAdd.CustomShoeDesignId 
                                                                                        && csd.IsDeleted == false 
                                                                                        && (csd.Status == CustomShoeDesignStatus.Public 
                                                                                        || csd.Status == CustomShoeDesignStatus.Private));
                if (customShoeDesign == null)
                {
                    throw new InvalidOperationException($"CustomShoeDesign with ID {orderDetailToAdd.CustomShoeDesignId} not found.");
                }
                
                var totalAmount = await CalculateTotalAmount(customShoeDesign);

                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    CustomShoeDesignId = orderDetailToAdd.CustomShoeDesignId,
                    SizeId = orderDetailToAdd.SizeId,
                    Quantity = orderDetailToAdd.Quantity,
                    TotalPrice = totalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ManufacturerId = orderDetailToAdd.ManufacturerId
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            
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
            if(request.Status == null)
            {
                throw new InvalidOperationException("Status is required");
            }
            if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
            {
                throw new InvalidOperationException("Invalid order status");
            }
            if (request.Id <= 0)
            {
                throw new InvalidOperationException("Id can not be 0");
            }
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

        private async Task<int> CalculateTotalAmount(CustomShoeDesign design)
    {
        var template = await _customShoeDesignTemplateRepository.FindAsync(design.CustomShoeDesignTemplateId);
        var templatePrice = template?.Price ?? 0;

        int servicesPrice = 0;
        if (design.DesignServices != null && design.DesignServices.Any())
        {
            servicesPrice = design.DesignServices.Sum(ds => ds.Service?.Price ?? 0);
        }

        return templatePrice + servicesPrice + design.DesignerMarkup;
    }
        #endregion
    }
}