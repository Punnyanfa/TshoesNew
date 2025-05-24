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
    public class AuthServiceLoginTests
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

        public AuthServiceLoginTests()
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
        public async Task Login_ValidCredentials()
        {
            var user = new User { Id = 1, Email = "phamthuan@gmail.com", PasswordHash = "hashedPassword", IsBanned = false };
            var request = new UserLoginRequest { Email = "phamthuan@gmail.com", Password = "thuan@1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.Password, user.PasswordHash)).Returns(true);
            _tokenServiceMock.Setup(x => x.GetToken(user)).Returns("token123");

            var result = await _authService.Login(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Login successful", result.Message);
        }

        [Fact]
        public async Task Login_EmptyEmail()
        {
            var request = new UserLoginRequest { Email = "", Password = "thuan@1234" };

            var result = await _authService.Login(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Invalid email or password", result.Message);
     
        }

        [Fact]
        public async Task Login_InvalidEmailFormat()
        {
            var request = new UserLoginRequest { Email = "thuan@gmail", Password = "thuan@1234" };
            var result = await _authService.Login(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Invalid email or password", result.Message);

        }

        [Fact]
        public async Task Login_NonExistentEmail()
        {
            var request = new UserLoginRequest { Email = "abc@gmail.com", Password = "thuan@1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);

            var result = await _authService.Login(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public async Task Login_EmailNoRole()
        {
            // Arrange
            var request = new UserLoginRequest { Email = "thuanpyse@fit.edu.vn", Password = "thuan@1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
            // Act
            var result = await _authService.Login(request);
            // Assert
            Assert.Equal(500, result.Code);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public async Task Login_EmptyPassword()
        {
            var request = new UserLoginRequest { Email = "phamthuan@gmail.com", Password = "" };

            var result = await _authService.Login(request);

            Assert.Equal(500, result.Code);
            Assert.Equal("Password can not be empty", result.Message);
        }

        [Fact]
        public async Task Login_InvalidPassword()
        {
            var user = new User { Id = 1, Email = "phamthuan@gmail.com", PasswordHash = "hashedPassword", IsBanned = false };
            var request = new UserLoginRequest { Email = "phamthuan@gmail.com", Password = "wrong12344555" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.Password, user.PasswordHash)).Returns(false);
            _tokenServiceMock.Setup(x => x.GetToken(user)).Returns("token123");
            
            var result = await _authService.Login(request);
            
            Assert.Equal(500, result.Code);
            Assert.Equal("Invalid email or password", result.Message);
       
        }

        [Fact]
        public async Task Login_PasswordLessThanEight()
        {
            var user = new User { Id = 1, Email = "phamthuan@gmail.com", PasswordHash = "hashedPassword", IsBanned = false };
            var request = new UserLoginRequest { Email = "phamthuan@gmail.com", Password = "pass" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.Password, user.PasswordHash)).Returns(false);
          
            var result = await _authService.Login(request);
         
            Assert.Equal(500, result.Code);
            Assert.Equal("Password can not less than 8 character ", result.Message);
        }

        [Fact]
        public async Task Login_BannedUser()
        {
            var user = new User { Id = 1, Email = "phamthuan@gmail.com", PasswordHash = "hashedPassword", IsBanned = true };
            var request = new UserLoginRequest { Email = "phamthuan@gmail.com", Password = "thuan@1234" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHashingServiceMock.Setup(x => x.VerifyHashedPassword(request.Password, user.PasswordHash)).Returns(true);
        
            var result = await _authService.Login(request);
         
            Assert.Equal(401, result.Code);
            Assert.Equal("This account has been banned", result.Message);
        }

        [Fact]
        public async Task Login_InvalidEmailAndPassword()
        {
          
            var request = new UserLoginRequest { Email = "thuan@gmail", Password = "wrong" };
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
           
            var result = await _authService.Login(request);
           
            Assert.Equal(500, result.Code);
            Assert.Equal("Password can not less than 8 character ", result.Message);
        }
    }
}