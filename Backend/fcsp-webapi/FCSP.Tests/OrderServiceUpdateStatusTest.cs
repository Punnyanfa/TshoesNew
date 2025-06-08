using FCSP.DTOs.Order;
using FCSP.Repositories.Interfaces;
using FCSP.Services.OrderService;
using FCSP.Services.PaymentService;
using Moq;
using FCSP.Common.Enums;
using FCSP.Models.Entities;

namespace FCSP.Tests
{
    public class OrderServiceUpdateStatusTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IVoucherRepository> _voucherRepositoryMock;
        private readonly Mock<IShippingInfoRepository> _shippingInfoRepositoryMock;
        private readonly Mock<ICustomShoeDesignRepository> _customShoeDesignRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<ICustomShoeDesignTemplateRepository> _customShoeDesignTemplateRepositoryMock;
        private readonly Mock<IRatingRepository> _ratingRepositoryMock;
        private readonly OrderService _orderService;

        public OrderServiceUpdateStatusTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _paymentServiceMock = new Mock<IPaymentService>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _voucherRepositoryMock = new Mock<IVoucherRepository>();
            _shippingInfoRepositoryMock = new Mock<IShippingInfoRepository>();
            _customShoeDesignRepositoryMock = new Mock<ICustomShoeDesignRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _customShoeDesignTemplateRepositoryMock = new Mock<ICustomShoeDesignTemplateRepository>();
            _ratingRepositoryMock = new Mock<IRatingRepository>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _orderDetailRepositoryMock.Object,
                _paymentServiceMock.Object,
                _paymentRepositoryMock.Object,
                _voucherRepositoryMock.Object,
                _shippingInfoRepositoryMock.Object,
                _customShoeDesignRepositoryMock.Object,
                _userRepositoryMock.Object,
                _sizeRepositoryMock.Object,
                _customShoeDesignTemplateRepositoryMock.Object,
                _ratingRepositoryMock.Object
            );
        }


        [Fact]
        public async Task OrderServiceUpdateStatus_IdIsZero()
        {
            var request = new UpdateOrderStatusRequest
            {
                Id = 0,
                Status = OrderStatus.Processing
            };

            var exception = await _orderService.UpdateOrderStatus(request);

            Assert.Equal(400, exception.Code);
            Assert.Contains("Id can not be 0", exception.Message);
        }

        [Fact]
        public async Task OrderServiceUpdateStatus_OrderNotFound()
        {
            var request = new UpdateOrderStatusRequest
            {
                Id = 1,
                Status = OrderStatus.Processing
            };

            _orderRepositoryMock.Setup(x => x.FindAsync()).ReturnsAsync((Order)null);

            var exception = await _orderService.UpdateOrderStatus(request);
            Assert.Equal(404, exception.Code);
            Assert.Contains($"{request.Id} not found", exception.Message);
        }

        [Fact]
        public async Task OrderServiceUpdateStatus_InvalidStatus()
        {
            var request = new UpdateOrderStatusRequest
            {
                Id = 1,
                Status = (OrderStatus)999
            };
            var order = new Order { Id = 1, Status = OrderStatus.Pending };

            _orderRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(order);
            var exception = await _orderService.UpdateOrderStatus(request);

            Assert.Equal(400, exception.Code);
            Assert.Contains("Invalid order status", exception.Message);
        }
        [Fact]
        public async Task OrderServiceUpdateStatus_Success()
        {
            var request = new UpdateOrderStatusRequest
            {
                Id = 1,
                Status = OrderStatus.Pending
            };
            var order = new Order { Id = 1, Status = OrderStatus.Refunded };
            _orderRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(order);

            var response = await _orderService.UpdateOrderStatus(request);

            Assert.Equal(200, response.Code);
            Assert.Contains("successfully", response.Message);

        }
    }
}
