using FCSP.Common.Enums;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.ManufacturerService;
using Moq;

namespace FCSP.Tests
{
    public class ManufacturerServiceAddTests
    {
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerServiceAddTests()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _manufacturerService = new ManufacturerService(_manufacturerRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task AddManufacturer_UserIdIsZero()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId  = 0, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("UserId is required", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_UserDoesNotExist()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1123, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1123)).ReturnsAsync((User)null);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("User not found", result.Message);
            Assert.Null(result.Data);
        }     

        [Fact]
        public async Task AddManufacturer_DescriptionIsEmpty()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Description can not be empty", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacture_DescriptionIsLessThanFiveWords()
        {
            var request = new AddManufacturerRequest { UserId = 1, Description = "Short desc", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            var result = await _manufacturerService.AddManufacturer(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Description must be greater than 5 words", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_DescriptionIsMoreThanFiftyWords()
        {
            var longDescription = string.Join(" ", new string[51].Select((_, i) => $"word{i}"));
            var request = new AddManufacturerRequest { UserId = 1, Description = longDescription, CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            var result = await _manufacturerService.AddManufacturer(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Description must be less than 50 words", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_CommissionRateIsLessThanFive()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "Highly recommended for GenZ style", CommissionRate = 4, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("commissionRate must be greater than 5", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_CommissionRateIsGreaterThanFifty()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "Highly recommended for GenZ style", CommissionRate = 51, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("commissionRate must be less than 50", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_StatusIsInvalid()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = 999 }; 
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("status is invalid", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AddManufacturer_AllInputsAreValid()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            var addedManufacturer = new Manufacturer { Id = 1, Description = request.Description, CreatedAt = DateTime.UtcNow };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync((Manufacturer)null);
            _manufacturerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Manufacturer>())).ReturnsAsync(addedManufacturer);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(201, result.Code);
            Assert.Equal("Manufacturer added successfully", result.Message);
            Assert.NotNull(result.Data);
            
        }

        [Fact]
        public async Task AddManufacturer_ManufacturerAlreadyExistsForUser()
        {
            // Arrange
            var request = new AddManufacturerRequest { UserId = 1, Description = "Highly recommended for GenZ style", CommissionRate = 10, Status = (int)ManufacturerStatus.Active };
            var user = new User { Id = 1, UserRole = UserRole.Manufacturer };
            var existingManufacturer = new Manufacturer { Id = 1, UserId = 1 };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(repo => repo.GetManufacturerByUserIdAsync(1)).ReturnsAsync(existingManufacturer);

            // Act
            var result = await _manufacturerService.AddManufacturer(request);

            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("User already has a Manufacturer", result.Message);
            Assert.Null(result.Data);
        }
    }
}