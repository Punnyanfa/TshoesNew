using FCSP.Repositories.Interfaces;
using FCSP.Services.ServiceService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.Tests
{
    public class ServiceServiceUpdateTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manuRepositoryMock;
        private readonly ServiceService _serviceService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        public ServiceServiceUpdateTests() 
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _manuRepositoryMock = new Mock<IManufacturerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _serviceService = new ServiceService(_serviceRepositoryMock.Object, _manuRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task ServiceServiceUpdate_IdIsZero()
        {

        }
    }
}
