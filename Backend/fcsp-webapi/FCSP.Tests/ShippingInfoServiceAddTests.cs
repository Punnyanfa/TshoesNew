using FCSP.Repositories.Interfaces;
using FCSP.Services.ShippingInfoService;
using FCSP.DTOs.ShippingInfo;
using Moq;
using FCSP.Models.Entities;



namespace FCSP.Tests
{
    public class ShippingInfoServiceAddTests
    {
        private readonly Mock<IShippingInfoRepository> _shippingInfoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ShippingInfoService _shippingInfoService;

        public ShippingInfoServiceAddTests()
        {
            _shippingInfoRepositoryMock = new Mock<IShippingInfoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _shippingInfoService = new ShippingInfoService(_shippingInfoRepositoryMock.Object, _userRepositoryMock.Object);
        }

       
        private string longString = string.Join(" ", new string[25].Select((i) => $"number{i}"));

        [Fact]
        public async Task ShippingInfoServiceAdd_IdNotFound()
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1123,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            _userRepositoryMock.Setup(x => x.FindAsync(request.UserId)).ReturnsAsync((User)null);
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(404, exception.Code);
            Assert.Contains($"{request.UserId} not found", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_IdIsZero()
        {
            var request = new AddShippingInfoRequest { UserId = 0, Address = "123 Main St", City = "Test City" };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("User ID must be greater than 0", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_PhoneNumberIsNull()
        {
            var request = new AddShippingInfoRequest { UserId = 1, PhoneNumber = null };
            var exception = await _shippingInfoService.AddShippingInfo(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Phone number is required", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_PhoneNumberDoesNotFormat()
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1123,
                PhoneNumber = "asdasdadcccc",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Phone number is not correct", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_PhoneNumberExceedTwentyFive()
        {
            var isPhoneNumber = new string('0', 26);
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = isPhoneNumber,
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            _userRepositoryMock.Setup(x => x.FindAsync(request.UserId)).ReturnsAsync(new User());
            var exception = await _shippingInfoService.AddShippingInfo(request);
      
            Assert.Equal(400, exception.Code);
            Assert.Equal("Phone number is not correct", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_AddressIsNull() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = null,
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Address is required", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_AddressILessThanFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            }; 
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Address must be at least 5 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_AddressIsExceedTwentyFive() 
        {         
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = longString,
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Address must be less than 25 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_CityIsNull() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = null,
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("City is required", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_DistrictIsLessThanFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test",
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("District must be at least 5 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_DistrictIsExceedTwentyFive() 
        {          
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = longString,
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("District must be less than 25 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_DistrictIsNull()
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = null,
                Ward = "Test Ward",
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("District is required", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_WardIsLessThanFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test",
                Country = "Test Country"
            };
          
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Ward must be at least 5 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_WardIsExceedTwentyFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = longString,
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Ward must be less than 25 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_WardIsNull()
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = null,
                Country = "Test Country"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Ward is required", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_CountryIsLessThanFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = "Test"
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Country must be at least 5 characters", exception.Message);
        }
        [Fact]
        public async Task ShippingInfoServiceAdd_CountryIsExceedTwentyFive() 
        {
            var request = new AddShippingInfoRequest
            {
                UserId = 1,
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                City = "Test City",
                District = "Test District",
                Ward = "Test Ward",
                Country = longString
            };
            var exception = await _shippingInfoService.AddShippingInfo(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Country must be less than 25 characters", exception.Message);
        }       
      
    }
}
