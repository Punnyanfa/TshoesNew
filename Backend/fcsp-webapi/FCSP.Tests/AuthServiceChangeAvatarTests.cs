using FCSP.Repositories.Interfaces;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using FCSP.Services.Auth;
using Moq;
using FCSP.Models.Entities;
using Microsoft.Extensions.Configuration;
using FCSP.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http.Internal;

namespace FCSP.Tests
{
    public class AuthServiceChangeAvatarTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IDesignerRepository> _designerRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserOtpRepository> _userOtpRepositoryMock;
        private readonly TestAuthService _authService;

        public AuthServiceChangeAvatarTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHashingServiceMock = new Mock<IPasswordHashingService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _configurationMock = new Mock<IConfiguration>();
            _designerRepositoryMock = new Mock<IDesignerRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _userOtpRepositoryMock = new Mock<IUserOtpRepository>();

            // Mock Azure Storage configuration
            _configurationMock.Setup(c => c["AzureStorage:ConnectionString"]).Returns("UseDevelopmentStorage=true");
            _configurationMock.Setup(c => c["AzureStorage:ImagesContainer"]).Returns("images");

            _authService = new TestAuthService(
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
        private class TestAuthService : AuthService
        {
            private readonly Func<string, byte[], Task<string>> _uploadToAzureStorageMock;

            public TestAuthService(
                IPasswordHashingService passwordHashingService,
                ITokenService tokenService,
                IUserRepository userRepository,
                IConfiguration configuration,
                IDesignerRepository designerRepository,
                IManufacturerRepository manufacturerRepository,
                IEmailService emailService,
                IUserOtpRepository userOtpRepository)
                : base(passwordHashingService, tokenService, userRepository, configuration, designerRepository, manufacturerRepository, emailService, userOtpRepository)
            {
                _uploadToAzureStorageMock = (fileName, fileBytes) =>
                {
                    Console.WriteLine($"Uploading to Azure Storage: {fileName}");
                    return Task.FromResult($"https://mockstorage.blob.core.windows.net/images/{fileName}");
                };
            }
        }
        private IFormFile CreateValidImageFile()
        {
          
            byte[] jpegBytes = new byte[] {};
            var stream = new MemoryStream(jpegBytes);
            stream.Position = 0;
            var formFile = new FormFile(stream, 0, stream.Length, "avatar", "avatar.jpg")
            {
                Headers = new HeaderDictionary { { "Content-Type", "image/jpeg" } },
                ContentType = "image/jpeg"
            };
            Console.WriteLine($"Created FormFile with ContentType: {formFile.ContentType}, Length: {formFile.Length}");
            return formFile;
        }
        private IFormFile CreateInvalidImageFile()
        {
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("This is not an image"));
            var formFile = new FormFile(stream, 0, stream.Length, "avatar", "avatar.txt")
            {
                Headers = new HeaderDictionary { { "Content-Type", "text/plain" } },
                ContentType = "text/plain"
            };
            Console.WriteLine($"Created FormFile with ContentType: {formFile.ContentType}");
            return formFile;
        }

        [Fact(Timeout = 1000)]
        public async Task UpdateUserAvatar_IdIsZero()
        {
            var request = new UpdateUserAvatarRequest { Id = 0, Avatar = CreateValidImageFile() };

            var result = await _authService.UpdateUserAvatar(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id can not be 0", result.Message);
        }

        [Fact(Timeout = 1000)]
        public async Task UpdateUserAvatar_IdDoesNotExist()
        {
            var request = new UpdateUserAvatarRequest { Id = 1234, Avatar = CreateValidImageFile() };
            _userRepositoryMock.Setup(r => r.FindAsync(new object[] { 1234L })).ReturnsAsync((User)null);

            var result = await _authService.UpdateUserAvatar(request);
            
            Assert.Equal(404, result.Code);
            Assert.Equal("User with ID 1234 not found", result.Message);
        }

        [Fact(Timeout = 1000)]
        public async Task UpdateUserAvatar_AvatarIsNull()
        {
            // Arrange
            var user = new User { Id = 1, AvatarImageUrl = null };
            var request = new UpdateUserAvatarRequest { Id = 1, Avatar = null };
            _userRepositoryMock.Setup(r => r.FindAsync(new object[] { 1L })).ReturnsAsync(user);
            // Act
            var result = await _authService.UpdateUserAvatar(request);
            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("Avatar is required", result.Message);
        }

        [Fact(Timeout = 1000)]
        public async Task UpdateUserAvatar_InvalidImageFormat()
        {
            // Arrange
            var user = new User { Id = 1, AvatarImageUrl = null };
            var request = new UpdateUserAvatarRequest { Id = 1, Avatar = CreateInvalidImageFile() };
            _userRepositoryMock.Setup(r => r.FindAsync(new object[] { 1L })).ReturnsAsync(user);
            // Act
            var result = await _authService.UpdateUserAvatar(request);
            // Assert
            Assert.Equal(400, result.Code);
            Assert.Equal("Only JPEG or PNG images are allowed", result.Message);

        }
      
    }
}