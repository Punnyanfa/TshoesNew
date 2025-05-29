using FCSP.Repositories.Interfaces;
using FCSP.Services.VoucherService;
using Microsoft.Extensions.Configuration;
using Moq;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.DTOs.Cart;
using FCSP.Repositories.Implementations;

namespace FCSP.Tests
{
    public class VoucherSeriveRemoveTests
    {
        private readonly Mock<IVoucherRepository> _voucherRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly VoucherService _voucherService;

        public VoucherSeriveRemoveTests()
        {
            _voucherRepositoryMock = new Mock<IVoucherRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _voucherService = new VoucherService(_voucherRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task VoucherServiceRemove_IdNotFound()
        {          
            var request = new DeleteVoucherRequest {Id = 1234};
            _voucherRepositoryMock.Setup(x => x.FindAsync(123)).ReturnsAsync((Voucher)null);
            
            var result = await _voucherService.DeleteVoucher(request);
            
            Assert.Equal(404, result.Code);
            Assert.Equal("Voucher not found", result.Message);
        }
        [Fact]
        public async Task VoucherServiceRemove_IdIsZero()
        {
            var request = new DeleteVoucherRequest { Id = 0 };
            _voucherRepositoryMock.Setup(x => x.FindAsync(0)).ReturnsAsync((Voucher)null);

            var result = await _voucherService.DeleteVoucher(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Id must be greater than 0", result.Message);
        }
        [Fact]
        public async Task VoucherServiceRemove_ValidRequest()
        {
            var voucher = new Voucher { Id = 1, VoucherName = "MUNGLEQUOCKHANH", CreatedAt = DateTime.UtcNow };
            var request = new DeleteVoucherRequest { Id = 1 };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(voucher);
            var exception = await _voucherService.DeleteVoucher(request);

            Assert.Equal(200, exception.Code);
            Assert.Equal("Voucher deleted successfully", exception.Message);
        }
    }
}
