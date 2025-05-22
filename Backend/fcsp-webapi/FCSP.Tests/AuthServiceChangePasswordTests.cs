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
    public class AuthServiceUpdateUserPasswordTests
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

        public AuthServiceUpdateUserPasswordTests()
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
        public async Task UpdateUserPassword_ValidInput()
        {
            var user = new User { Id = 1234, Name = "Thuan", Email = "thuan123@gmail.com", PasswordHash = "hashedThuan1234", UserRole = UserRole.Customer };
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "newpass1234" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash)).Returns(true);
            _passwordHashingServiceMock.Setup(x => x.GetHashedPassword(request.NewPassword)).Returns("hashedNewpass1234");
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_EmptyId()
        {
            var request = new UpdateUserPasswordRequest { Id = 0, CurrentPassword = "thuan1234", NewPassword = "newpass1234" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id required", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_IdExists()
        {
            var user = new User { Id = 1234, Name = "Thuan", Email = "thuan123@gmail.com", PasswordHash = "hashedThuan1234", UserRole = UserRole.Customer };
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "newpass1234" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash)).Returns(true);
            _passwordHashingServiceMock.Setup(x => x.GetHashedPassword(request.NewPassword)).Returns("hashedNewpass1234");
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_IdNotFound()
        {
            var request = new UpdateUserPasswordRequest { Id = 12345, CurrentPassword = "thuan1234", NewPassword = "newpass1234" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync((User)null);

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id not found", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_EmptyCurrentPassword()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "", NewPassword = "newpass1234" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Current password is required", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_CurrentPasswordMatches()
        {
            var user = new User { Id = 1234, Name = "Thuan", Email = "thuan123@gmail.com", PasswordHash = "hashedThuan1234", UserRole = UserRole.Customer };
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "newpass1234" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash)).Returns(true);
            _passwordHashingServiceMock.Setup(x => x.GetHashedPassword(request.NewPassword)).Returns("hashedNewpass1234");
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_CurrentPasswordNotMatch()
        {
            var user = new User { Id = 1234, Name = "Thuan", Email = "thuan123@gmail.com", PasswordHash = "hashedThuan1234", UserRole = UserRole.Customer };
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "wrong1234", NewPassword = "newpass1234" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash)).Returns(false);

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Current password does not match", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_CurrentPasswordNotFormat()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan", NewPassword = "newpass1234" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Current password not format", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_CurrentPasswordLessThanEight()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan12", NewPassword = "newpass1234" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Current password cannot be less than 8 character", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_CurrentPasswordGreaterThanTwenty()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan123456789012345678901", NewPassword = "newpass1234" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Current password cannot be greater than 20 character", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_EmptyNewPassword()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("New password cannot be empty", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_NewPasswordNotFormat()
        {          
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "vuongggg" };
            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("New password does not format", result.Message);        
        }

        [Fact]
        public async Task UpdateUserPassword_NewPasswordLessThanEight()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "vuong12" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("New password less than 8 character", result.Message);
        }

        [Fact]
        public async Task UpdateUserPassword_NewPasswordGreaterThanTwenty()
        {
            var request = new UpdateUserPasswordRequest { Id = 1234, CurrentPassword = "thuan1234", NewPassword = "vuong123456789012345678901" };

            var result = await _authService.UpdateUserPassword(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("New password greater than 20 character", result.Message);
        }
    }
}