using FCSP.Common.Enums;
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
    public class AuthServiceUpdateUserStatusTests
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

        public AuthServiceUpdateUserStatusTests()
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
        public async Task UpdateUserStatus_IdNotFound()
        {
            var request = new UpdateUserStatusRequest { Id = 9999, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(9999))
                .ReturnsAsync((User)null);

            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(404, result.Code);
            Assert.Equal("User with ID 9999 not found", result.Message);
        }
        [Fact]
        public async Task UpdateUserStatus_UserIsAdmin()
        {
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Admin,
                IsBanned = false,
                UpdatedAt = DateTime.UtcNow
            };
            var request = new UpdateUserStatusRequest { Id = 1, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);

            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Admin accounts cannot be banned", result.Message);            
        }
        [Fact]
        public async Task UpdateUserStatus_ValidCustomerUser()
        {
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Customer,
                IsBanned = false,
                UpdatedAt = DateTime.UtcNow
            };
            var request = new UpdateUserStatusRequest { Id = 1, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("User status updated successfully", result.Message);
            Assert.NotNull(result.Data);
           
        }
        [Fact]
        public async Task UpdateUserStatus_ValidDesignerUser()
        {
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Designer,
                IsBanned = false,
                UpdatedAt = DateTime.UtcNow
            };
            var designer = new Designer
            {
                UserId = 1,
                Status = DesignerStatus.Active
            };
            var request = new UpdateUserStatusRequest { Id = 1, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            _designerRepositoryMock.Setup(x => x.GetDesignerByUserIdAsync(1))
                .ReturnsAsync(designer);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _designerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Designer>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("User status updated successfully", result.Message);
            
        }
        [Fact]
        public async Task UpdateUserStatus_ValidManufacturerUser()
        {
            var user = new User
            {
                Id = 1,
                UserRole = UserRole.Manufacturer,
                IsBanned = false,
                UpdatedAt = DateTime.UtcNow
            };
            var manufacturer = new Manufacturer
            {
                UserId = 1,
                Status = ManufacturerStatus.Active
            };
            var request = new UpdateUserStatusRequest { Id = 1, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 1)))
                .ReturnsAsync(user);
            _manufacturerRepositoryMock.Setup(x => x.GetManufacturerByUserIdAsync(1))
                .ReturnsAsync(manufacturer);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _manufacturerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Manufacturer>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("User status updated successfully", result.Message);
   
           
        }
     
    }
}