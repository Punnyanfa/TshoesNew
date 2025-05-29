using FCSP.Services.ServiceService;
using FCSP.Repositories.Interfaces;
using Moq;
using FCSP.DTOs.Service;
using FCSP.Models.Entities;


namespace FCSP.Tests
{
    public class ServiceServiceAddTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manuRepositoryMock;
        private readonly ServiceService _serviceService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public ServiceServiceAddTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _manuRepositoryMock = new Mock<IManufacturerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _serviceService = new ServiceService(_serviceRepositoryMock.Object, _manuRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task ServiceServiceAdd_ComponentIsNullOrEmpty()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active }; 
            var addService = new AddService { Component = " ", Type = "Type1", Price = 100, ManufacturerId = manufacturer.Id };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Component cannot be null or empty", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_ComponentLessThanFour()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
            var addService = new AddService { Component = "ABC", Type = "Type1", Price = 100, ManufacturerId = 1 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);

            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Component must be at least 4 characters", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_ComponentGreaterThanTwenty()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
            var addService = new AddService { Component = new string('A', 21), Type = "Type1", Price = 100, ManufacturerId = manufacturer.Id };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Component must be less than 20 characters", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_TypeIsNullOrEmpty()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
       
            var addService = new AddService { Component = "Component1", Type = null, Price = 100, ManufacturerId = 1 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Type cannot be null or empty", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_InvalidType()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
         
            var addService = new AddService { Component = "Component1", Type = "InvalidType@#@$", Price = 100, ManufacturerId = 1 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Type can only contain letters, digits, and spaces", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_PriceIsNegative()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
            var addService = new AddService { Component = "Component1", Type = "Type1", Price = -100, ManufacturerId = 1 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Price cannot be negative", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_ManufacturerIdIsZero()
        {
          
            var addService = new AddService { Component = "Component1", Type = "Type1", Price = 100, ManufacturerId = 0 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
           
            var result = await _serviceService.AddService(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("ManufacturerId can not be 0", result.Message);
        }
        [Fact]
        public async Task ServiceServiceAdd_ManufacturerIdNotFound()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };
            var addService = new AddService { Component = "Component1", Type = "Type1", Price = 100, ManufacturerId = 12314 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };

            _manuRepositoryMock.Setup(x => x.FindAsync()).ReturnsAsync((Manufacturer)null);
            var result = await _serviceService.AddService(request);
            Assert.Equal(404, result.Code);
            Assert.Equal("Manufacturer not found", result.Message);
        }
        [Fact] 
        public async Task ServiceServiceAdd_Success()
        {
            var user = new User { Id = 1, UserRole = Common.Enums.UserRole.Manufacturer };
            var manufacturer = new Manufacturer { Id = 1, UserId = user.Id, Status = Common.Enums.ManufacturerStatus.Active };

            var addService = new AddService { Component = "Component1", Type = "Valid Type", Price = 100, ManufacturerId = 1 };
            var request = new AddServiceRequest
            {
                AddServices = new List<AddService> { addService }
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _manuRepositoryMock.Setup(x => x.GetManufacturerWithDetailsAsync(addService.ManufacturerId)).ReturnsAsync(manufacturer);
            var result = await _serviceService.AddService(request);
            Assert.Equal(201, result.Code);
            Assert.Contains("Success", result.Message);
        }
    }
}
