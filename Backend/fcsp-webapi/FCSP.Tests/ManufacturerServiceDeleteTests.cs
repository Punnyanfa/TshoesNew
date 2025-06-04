using FCSP.Common.Enums;

using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ManufacturerService;
using Moq;


namespace FCSP.Tests
{
    public class ManufacturerServiceDeleteTests
    {
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ManufacturerService _manufacturerService;
        public ManufacturerServiceDeleteTests()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _manufacturerService = new ManufacturerService(_manufacturerRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task DeleteManufacturer_InvalidId()
        {
            // Arrange
            var request = new GetManufacturerRequest { Id = 0 };

            // Act
            var result = await _manufacturerService.DeleteManufacturer(request);

            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("Manufacturer ID must be greater than 0", result.Message);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task DeleteManufacturer_ManufacturerNotFound()
        {
            // Arrange
            var request = new GetManufacturerRequest { Id = 1234 };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1234)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.DeleteManufacturer(request);

            // Assert
            Assert.Equal(404, result.Code);
            Assert.Equal("Manufacturer not found", result.Message);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task DeleteManufacturer_ValidRequest()
        {
            // Arrange
            var manufacturer = new Manufacturer
            {
                Id = 1,
                IsDeleted = false,
                Status = ManufacturerStatus.Active,
                UpdatedAt = DateTime.Now
            };
            var request = new GetManufacturerRequest { Id = 1 };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);
            _manufacturerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Manufacturer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _manufacturerService.DeleteManufacturer(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("Manufacturer marked as inactive successfully", result.Message);
            Assert.True(result.Data);
            Assert.True(manufacturer.IsDeleted);
            Assert.Equal(ManufacturerStatus.Inactive, manufacturer.Status);
        }
    }
}

