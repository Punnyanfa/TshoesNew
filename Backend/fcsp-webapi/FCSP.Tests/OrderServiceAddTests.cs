using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.DTOs.OrderDetail;
using FCSP.DTOs.Payment;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.OrderService;
using FCSP.Services.PaymentService;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;

namespace FCSP.Tests
{
    public class OrderServiceAddTest
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
        private readonly OrderService _orderService;

        public OrderServiceAddTest()
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
                _customShoeDesignTemplateRepositoryMock.Object);
        }

        [Fact]
        public async Task OrderService_UserIdIsZero()
        {
            var request = new AddOrderRequest
            {
                UserId = 0,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Contains("can not be 0", exception.Message);
        }

        [Fact]
        public async Task OrderService_NonExistingUserId()
        {
            var request = new AddOrderRequest
            {
                UserId = 1234,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 9999))).ReturnsAsync((User)null);

            var exception = await  _orderService.AddOrder(request);

            Assert.Equal(404, exception.Code);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public async Task OrderService_NullShippingInfoId()
        {
           var user = new User { Id = 1, AvatarImageUrl = null };
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 0,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1))).ReturnsAsync(user);

            var exception = await  _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("shippingInforId is require", exception.Message);
        }

        [Fact]
        public async Task OrderService_NonExistingShippingInfoId()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 12345,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1))).ReturnsAsync(new User());
            _shippingInfoRepositoryMock.Setup(x => x.FindAsync(12345)).ReturnsAsync((ShippingInfo)null);


            var exception = await _orderService.AddOrder(request);

            Assert.Equal(404, exception.Code);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public async Task OrderService_NullPaymentMethod()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = 0
            };

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("paymentMethodId is require", exception.Message);
        }

        [Fact]
        public async Task OrderService_InvalidPaymentMethod()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = (PaymentMethod)5
            };

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("paymentMethod is not valid", exception.Message);
        }

        [Fact]
        public async Task OrderService_CustomShoeDesignIdIsZero()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 0, Quantity = 1, SizeId = 1, ManufacturerId = 1 }, 
                PaymentMethod = PaymentMethod.Wallet
            };

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("customeShoeDesignId is require", exception.Message);
        }
    
        [Fact]
        public async Task OrderService_NullQuantity()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1,Quantity = 0, SizeId = 1, ManufacturerId = 1 }, 
                PaymentMethod = PaymentMethod.Wallet
            };
          
            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("quantity is require", exception.Message);
        }

        [Fact]
        public async Task OrderService_InvalidQuantity()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 2, SizeId = 1, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };
            var customShoeDesign = new CustomShoeDesign { Id = 1, CustomShoeDesignTemplateId = 1, DesignerMarkup = 10 };
            var queryableCustomShoeDesigns = new List<CustomShoeDesign> { customShoeDesign }.AsQueryable();
            _customShoeDesignRepositoryMock.Setup(x => x.GetAll()).Returns(queryableCustomShoeDesigns);


            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Only 1 custom shoe design for 1 order", exception.Message);
        }

        [Fact]
        public async Task OrderService_SizeIdIsZero()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 0, ManufacturerId = 1 }, 
                PaymentMethod = PaymentMethod.Wallet
            };

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("sizeId can not be 0", exception.Message);
        }

        [Fact]
        public async Task OrderService_NonExistingSizeId()
        {
            var request = new AddOrderRequest
            {
                UserId = 1,
                ShippingInfoId = 1,
                VoucherId = null,
                OrderDetail = new OrderDetailRequestDto { CustomShoeDesignId = 1, Quantity = 1, SizeId = 23455, ManufacturerId = 1 },
                PaymentMethod = PaymentMethod.Wallet
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1))).ReturnsAsync(new User());
            _shippingInfoRepositoryMock.Setup(x => x.FindAsync(1)).ReturnsAsync(new ShippingInfo());
            _sizeRepositoryMock.Setup(x => x.FindAsync(23455)).ReturnsAsync((Size)null);

            var exception = await _orderService.AddOrder(request);

            Assert.Equal(404, exception.Code);
            Assert.Contains(" not found", exception.Message);
        }
    }
}