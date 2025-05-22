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
    public class AuthServiceRegisterTests
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

        public AuthServiceRegisterTests()
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
        public async Task Register_ValidInput()
        {
            var user = new User { Id = 1, Name = "Thuan", Email = "thuan123@gmail.com", PasswordHash = "hashedPassword", UserRole = UserRole.Customer };
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuan1234", ConfirmPassword = "thuan1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _passwordHashingServiceMock.Setup(x => x.GetHashedPassword(request.Password)).Returns("hashedPassword");
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(user); 

            var result = await _authService.Register(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("User registered successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        [Fact]
        public async Task Register_EmptyName()
        {
            var request = new UserRegisterRequest { Name = "", Email = "thuan123@gmail.com", Password = "thuan1234", ConfirmPassword = "thuan1234" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Name cannot be empty", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_NonExistentName()
        {
            var user = new User { Id = 1, Name = "Thanh", Email = "thuan123@gmail.com", PasswordHash = "hashedPassword", UserRole = UserRole.Customer };
            var request = new UserRegisterRequest { Name = "Thanh", Email = "thuan123@gmail.com", Password = "thuan1234", ConfirmPassword = "thuan1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _passwordHashingServiceMock.Setup(x => x.GetHashedPassword(request.Password)).Returns("hashedPassword");
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(user); // Fix: Return Task<User>

            var result = await _authService.Register(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("User registered successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        [Fact]
        public async Task Register_EmptyEmail()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "", Password = "thuan1234", ConfirmPassword = "thuan1234" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Email can not be empty", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_InvalidEmailFormat()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123gmail.com", Password = "thuan1234", ConfirmPassword = "thuan1234" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Invalid email format", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_ExistingEmail()
        {
            var existingUser = new User { Id = 1, Email = "thuan123@gmail.com" };
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuan1234", ConfirmPassword = "thuan1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(existingUser);

            var result = await _authService.Register(request);

            Assert.Equal(409, result.Code);
            Assert.Equal("User with this email already exists", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_EmptyPassword()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "", ConfirmPassword = "" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Password cannot be empty", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_PasswordLessThanEight()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuan12", ConfirmPassword = "thuan12" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Password can not less than 8 character", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_PasswordGreaterThanTwenty()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuan123456789012345678901", ConfirmPassword = "thuan123456789012345678901" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Password can not greater than 20 character", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_InvalidPasswordFormat()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuannn@", ConfirmPassword = "thuannn@" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Password does not format", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task Register_NonMatchingConfirmPassword()
        {
            var request = new UserRegisterRequest { Name = "Thuan", Email = "thuan123@gmail.com", Password = "thuan1234", ConfirmPassword = "thuan12345" };

            var result = await _authService.Register(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Password not match", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }
    }
}