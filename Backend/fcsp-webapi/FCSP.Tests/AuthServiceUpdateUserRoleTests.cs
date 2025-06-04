using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Authentication;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.Auth;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FCSP.Tests
{
    public class AuthServiceUpdateUserRoleTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IDesignerRepository> _designerRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserOtpRepository> _userOtpRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceUpdateUserRoleTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _designerRepositoryMock = new Mock<IDesignerRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _passwordHashingServiceMock = new Mock<IPasswordHashingService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _configurationMock = new Mock<IConfiguration>();
            _emailServiceMock = new Mock<IEmailService>();
            _userOtpRepositoryMock = new Mock<IUserOtpRepository>();

            _authService = new AuthService(
                _passwordHashingServiceMock.Object,
                _tokenServiceMock.Object,
                _userRepositoryMock.Object,
                _configurationMock.Object,
                _designerRepositoryMock.Object,
                _manufacturerRepositoryMock.Object,
                _emailServiceMock.Object,
                _userOtpRepositoryMock.Object
            );
        }
        [Fact]
        public async Task UpdateUserRole_InvalidId()
        {
            var request = new UpdateUserRoleRequest
            {
                Id = 0,
                Role = UserRole.Designer,
                CommissionRate = 10
            };

            var result = await _authService.UpdateUserRole(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("User Id can not be 0", result.Message);
        }
        [Fact]
        public async Task UpdateUserRole_NonExistentId()
        {
            var request = new UpdateUserRoleRequest
            {
                Id = 9999,
                Role = UserRole.Designer,
                CommissionRate = 10
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 9999)))
                .ReturnsAsync((User)null);

            var result = await _authService.UpdateUserRole(request);

            Assert.Equal(404, result.Code);
            Assert.Equal($"User with ID {request.Id} not found", result.Message);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task UpdateUserRole_InvalidRole()
        {
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = (UserRole)10,
                CommissionRate = 10
            };           
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);

            var result = await _authService.UpdateUserRole(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Role is invalid", result.Message);
        }
        [Fact]
        public async Task UpdateUserRole_InvalidCommissionRate_LessThanFive()
        {
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = UserRole.Designer,
                CommissionRate = -5 
            };
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);

            var result = await _authService.UpdateUserRole(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("commissionRate can not less than 5", result.Message);
        }
        [Fact]
        public async Task UpdateUserRole_InvalidCommissionRate_GreaterThanFifty()
        {
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = UserRole.Designer,
                CommissionRate = 51
            };
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);

            var result = await _authService.UpdateUserRole(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("commissionRate can not greater than 50", result.Message);
        }
        public async Task UpdateUserRole_InvalidCommissionRate_empty()
        {             // Arrange
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = UserRole.Designer,
                CommissionRate = null
            };
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            // Act
            var result = await _authService.UpdateUserRole(request);
            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("commissionRate is require", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }
        [Fact]
        public async Task UpdateUserRole_ToDesigner_Success()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            var designer = new Designer
            {
                UserId = 1,
                Rating = 0,
                Description = string.Empty,
                Status = DesignerStatus.Active,
                CommissionRate = 10
            };
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = UserRole.Designer,
                CommissionRate = 10
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            _designerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Designer>()))
                .ReturnsAsync(designer); // Return Task<Designer>
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.UpdateUserRole(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("User role updated successfully", result.Message);
            Assert.NotNull(result.Data);
           
        }
        [Fact]
        public async Task UpdateUserRole_ToManufacturer_Success()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                UpdatedAt = DateTime.Now
            };
            var manufacturer = new Manufacturer
            {
                UserId = 1,
                Description = string.Empty,
                Status = ManufacturerStatus.Active,
                CommissionRate = 15
            };
            var request = new UpdateUserRoleRequest
            {
                Id = 1,
                Role = UserRole.Manufacturer,
                CommissionRate = 15
            };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Manufacturer>()))
                .ReturnsAsync(manufacturer);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.UpdateUserRole(request);

            // Assert
            Assert.Equal(200, result.Code);
            Assert.Equal("User role updated successfully", result.Message);
            Assert.NotNull(result.Data);
           
        }
    }
}