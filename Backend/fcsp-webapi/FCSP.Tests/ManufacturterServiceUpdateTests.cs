using FCSP.Common.Enums;

using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ManufacturerService;
using Moq;

namespace FCSP.Tests
{
    public class ManufacturerServiceUpdateTests
    {
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerServiceUpdateTests()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _manufacturerService = new ManufacturerService(_manufacturerRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateManufacturer_InvalidId()
        {
            // Arrange
            var request = new UpdateManufacturerRequest { Id = 0, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Manufacturer ID must be greater than 0", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_ManufacturerNotFound()
        {
            // Arrange
            var request = new UpdateManufacturerRequest { Id = 11234, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(11234)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Manufacturer not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_DescriptionEmpty()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Old Description", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Description can not be empty", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_DescriptionLessThanFiveWords()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Old Description", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "Test", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Description must be greater than 5 words", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_DescriptionMoreThanFiftyWords()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Old Description", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var longDescription = string.Join(" ", new string[51].Select((_, i) => $"word{i}"));
            var request = new UpdateManufacturerRequest { Id = 1, Description = longDescription, CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Description must be less than 50 words", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_CommissionRateLessThanFive()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "Highly recommended for GenZ style", CommissionRate = 3, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("commissionRate must be greater than 5", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_CommissionRateMoreThanFifty()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "Highly recommended for GenZ style", CommissionRate = 51, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("commissionRate must be less than 50", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_InvalidStatus()
        {
            // Arrange
            var manufacturer = new Manufacturer 
            { Id = 1, 
              Description = "Highly recommended for GenZ style", 
              CommissionRate = 10,
              Status = ManufacturerStatus.Active 
            };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = 10 }; 
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("status is invalid", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateManufacturer_ValidRequest()
        {
            // Arrange
            var manufacturer = new Manufacturer { Id = 1, Description = "Old Description", CommissionRate = 10, Status = ManufacturerStatus.Active };
            var request = new UpdateManufacturerRequest { Id = 1, Description = "Updated description with more than five words", CommissionRate = 20, Status = (int)ManufacturerStatus.Active };
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(1)).ReturnsAsync(manufacturer);
            _manufacturerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Manufacturer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _manufacturerService.UpdateManufacturer(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("Manufacturer updated successfully", result.Message);
            Assert.NotNull(result.Data);           
        }
    }
}