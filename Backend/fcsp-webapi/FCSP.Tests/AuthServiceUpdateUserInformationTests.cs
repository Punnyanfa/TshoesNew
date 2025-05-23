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
    public class AuthServiceUpdateUserInformationTests
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

        public AuthServiceUpdateUserInformationTests()
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
        public async Task UpdateUserInformation_ValidInput_Returns200()
        {
            var user = new User { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01", UserRole = UserRole.Customer };
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_EmptyId_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 0, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id is required", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_IdExists_Returns200()
        {
            var user = new User { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01", UserRole = UserRole.Customer };
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_IdNotFound_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 12345, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync((User)null);

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id not found", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_EmptyName_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Name is not in correct format (Name can not be empty)", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_CorrectNameFormat_Returns200()
        {
            var user = new User { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01", UserRole = UserRole.Customer };
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_IncorrectNameFormat_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "123abc", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Name is not in correct format (Name can not be empty)", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_NameLessThan5_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thua", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Name can not less than 5 characters", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_NameMoreThan25_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Pham Van Thanh Long Ha Noi", PhoneNumber = "1234567890", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Name can not greater than 25 characters", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_EmptyPhoneNumber_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("PhoneNumber can not be empty", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_IncorrectPhoneFormat_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "123abc456789", Gender = "Male", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Invalid phone format", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_EmptyGender_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "", Dob = "2000-01-01" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Gender can not be empty", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }

        [Fact]
        public async Task UpdateUserInformation_InvalidDob_Returns400()
        {
            var request = new UpdateUserInformationRequest { Id = 1234, Name = "Thuan Pham", PhoneNumber = "1234567890", Gender = "Male", Dob = "30/13/2006" };

            var result = await _authService.UpdateUserInformation(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Invalid date of birth", result.Message);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.Success);
        }
    }
}