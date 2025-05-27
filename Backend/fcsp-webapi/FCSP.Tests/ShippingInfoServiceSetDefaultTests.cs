using FCSP.Repositories.Interfaces;
using FCSP.Services.ShippingInfoService;
using FCSP.Models.Entities;
using Moq;
using FCSP.DTOs.ShippingInfo;

namespace FCSP.Tests
{
    public class ShippingInfoServiceSetDefaultTests
    {
        private readonly Mock<IShippingInfoRepository> _shippingInfoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ShippingInfoService _shippingInfoService;

        public ShippingInfoServiceSetDefaultTests()
        {
            _shippingInfoRepositoryMock = new Mock<IShippingInfoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _shippingInfoService = new ShippingInfoService(_shippingInfoRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task ShippingInfoServiceSetDefault_UserIdIsZero()
        {

            var request = new SetDefaultShippingInfoRequest { UserId = 0, Id = 1 };

            var result = await _shippingInfoService.SetDefaultShippingInfo(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("UserId can not be 0", result.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceSetDefault_UserIdNotFound()
        {
            var request = new SetDefaultShippingInfoRequest { UserId = 1, Id = 1 };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((User)null);
            var result = await _shippingInfoService.SetDefaultShippingInfo(request);
            Assert.Equal(404, result.Code);
            Assert.Contains($"{request.UserId} not found", result.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceSetDefault_IdIsZero()
        {
            var request = new SetDefaultShippingInfoRequest { UserId = 1, Id = 0 };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new User ());
            var result = await _shippingInfoService.SetDefaultShippingInfo(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Shipping info Id is required", result.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceSetDefault_ShippingInfoNotFound()
        {
            var request = new SetDefaultShippingInfoRequest { UserId = 1, Id = 1 };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(request.UserId)).ReturnsAsync(new User());
            _shippingInfoRepositoryMock.Setup(repo => repo.FindAsync(request.Id))
                .ReturnsAsync((ShippingInfo)null);
            var result = await _shippingInfoService.SetDefaultShippingInfo(request);
            Assert.Equal(404, result.Code);
            Assert.Equal($"Shipping information with ID {request.Id} not found or does not belong to user", result.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceSetDefault_UserIsNotShippingInfoOwner()
        {
            var shippingInfo = new ShippingInfo { UserId = 2, Id = 1 };
            var request = new SetDefaultShippingInfoRequest { UserId = 1, Id = 1 };
            _shippingInfoRepositoryMock.Setup(repo => repo.FindAsync(request.Id))
                .ReturnsAsync(shippingInfo);
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new User());
            var result = await _shippingInfoService.SetDefaultShippingInfo(request);
            Assert.Equal(404, result.Code);
            Assert.Equal($"Shipping information with ID {request.Id} not found or does not belong to user", result.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceSetDefault_Success()
        {
            var shippingInfo = new ShippingInfo { UserId = 1, Id = 2 };
            var request = new SetDefaultShippingInfoRequest { UserId = 1, Id = 1 };

            _shippingInfoRepositoryMock.Setup(repo => repo.FindAsync(request.Id))
                .ReturnsAsync(shippingInfo);
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(request.UserId)).ReturnsAsync(new User());

            var exception = await _shippingInfoService.SetDefaultShippingInfo(request);
            Assert.Equal(200, exception.Code);
            Assert.Contains("successfully", exception.Message);
        }
    }
}
