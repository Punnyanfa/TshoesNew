using FCSP.DTOs;
using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ShippingInfoService;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FCSP.Tests
{
    public class ShippingInfoServiceDeleteTests
    {
        private readonly Mock<IShippingInfoRepository> _shippingInfoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ShippingInfoService _shippingInfoService;

        public ShippingInfoServiceDeleteTests()
        {
            _shippingInfoRepositoryMock = new Mock<IShippingInfoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _shippingInfoService = new ShippingInfoService(_shippingInfoRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task DeleteShippingInfo_InvalidId()
        {
            // Arrange
            var request = new DeleteShippingInfoRequest { Id = 0 }; // Invalid ID

            // Act
            var result = await _shippingInfoService.DeleteShippingInfo(request);

            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("Shipping info Id is required", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task DeleteShippingInfo_NonExistentId()
        {
            // Arrange
            var request = new DeleteShippingInfoRequest { Id = 9999 };
            _shippingInfoRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 9999)))
                .ReturnsAsync((ShippingInfo)null);

            // Act
            var result = await _shippingInfoService.DeleteShippingInfo(request);

            // Assert
            Assert.Equal(404, result.Code);
            Assert.Equal("Shipping information with ID 9999 not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteShippingInfo_ExistingId()
        {
            // Arrange
            var shippingInfo = new ShippingInfo
            {
                Id = 123,
                UserId = 1,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var request = new DeleteShippingInfoRequest { Id = 123 };
            _shippingInfoRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 123)))
                .ReturnsAsync(shippingInfo);
            _shippingInfoRepositoryMock.Setup(x => x.DeleteAsync(123))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _shippingInfoService.DeleteShippingInfo(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("Shipping information deleted successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }
    }
}