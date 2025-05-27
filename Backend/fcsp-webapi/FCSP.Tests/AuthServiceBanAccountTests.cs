using FCSP.Repositories.Interfaces;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using FCSP.Services.Auth;
using FCSP.DTOs.Authentication;
using FCSP.Common.Enums;
using Moq;
using Microsoft.Extensions.Configuration;
using FCSP.Models.Entities;

namespace FCSP.Tests
{
    public class AuthServiceBanAccountTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IDesignerRepository> _designerRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserOtpRepository> _userOtpRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceBanAccountTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHashingServiceMock = new Mock<IPasswordHashingService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _configurationMock = new Mock<IConfiguration>();
            _designerRepositoryMock = new Mock<IDesignerRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
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
        public async Task AuthServiceBanAccount_UserIdIsZero()
        {
            var request = new UpdateUserStatusRequest { Id = 0, IsBanned = true };
            var result = await _authService.UpdateUserStatus(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id must be greater than 0", result.Message);
        }

        [Fact]
        public async Task AuthServiceBanAccount_UserNotFound()
        {
            var request = new UpdateUserStatusRequest { Id = 9999, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == 9999)))
                .ReturnsAsync((User)null);

            var result = await _authService.UpdateUserStatus(request);
            Assert.Equal(404, result.Code);
            Assert.Contains($"{request.Id} not found", result.Message);
        }
        [Fact]
        public async Task AuthServiceBanAccount_UserIsAdmin()
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
        public async Task AutServiceBanAccount_Success()
        {
            var user = new User
            {
                Id = 1,
                IsBanned = false,
            };
            var request = new UpdateUserStatusRequest { Id = 1, IsBanned = true };
            _userRepositoryMock.Setup(x => x.FindAsync(It.Is<object[]>(args => (long)args[0] == request.Id)))
                .ReturnsAsync(user);

            var exception = await _authService.UpdateUserStatus(request);

            Assert.Equal(200, exception.Code);
            Assert.Contains("successfully", exception.Message);
        }
    }
}

