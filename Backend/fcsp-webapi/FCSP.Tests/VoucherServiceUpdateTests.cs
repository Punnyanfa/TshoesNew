using FCSP.Repositories.Interfaces;
using FCSP.Services.VoucherService;
using Microsoft.Extensions.Configuration;
using Moq;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;


namespace FCSP.Tests
{
     public class VoucherServiceUpdateTests
    {
        private readonly Mock<IVoucherRepository> _voucherRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly VoucherService _voucherService;

        public VoucherServiceUpdateTests()
        {
            _voucherRepositoryMock = new Mock<IVoucherRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _voucherService = new VoucherService(_voucherRepositoryMock.Object, _configurationMock.Object);
        }
        [Fact]
        public async Task VoucherServiceUpdate_IdNotFound()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TESTCODE", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync((Voucher)null);
            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(404, result.Code);
            Assert.Equal("Voucher not found", result.Message);
        }

        [Fact]
        public async Task VoucherServiceUpdate_IdIsZero()
        {
            var request = new UpdateVoucherRequest { Id = 0, Code = "TESTCODE", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };

            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Id can not be zero", result.Message);
        }

        [Fact]
        public async Task VoucherServiceUpdate_CodeIsNull()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = null, DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher { Id = request.Id });
            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Code cannot be null or empty", result.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_CodeLessThanFive()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TEST", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher { Id = request.Id });
            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Code must be at least 5 characters", result.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_CodeGreaterThanTwenty()
        {
            var longString = new string('A', 21);
            var request = new UpdateVoucherRequest { Id = 1, Code = longString, DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher());
            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Code must be less than or equal to 20 characters", result.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_InvalidFormatCode()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "12345@#$", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher { Id = request.Id });
            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, result.Code);
            Assert.Equal("Code should not include special character", result.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_DiscountAmountIsZero()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TESTCODE", DiscountAmount = 0, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher());
            var exception  = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be greater than 0", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_DiscountAmountIsNegative()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TESTCODE", DiscountAmount = -10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher());
            var exception = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be greater than 0", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_DiscountAmountGreaterThanThirty()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TESTCODE", DiscountAmount = 35, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher());
            var exception = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be less than or equal to 30", exception.Message);
        }     
        [Fact]
        public async Task VoucherServiceUpdate_ExpiryDateIsInThePast()
        {
            var request = new UpdateVoucherRequest { Id = 1, Code = "TESTCODE", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(-1) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(new Voucher());
            var exception = await _voucherService.UpdateVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Expiry date cannot be in the past", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceUpdate_Success()
        {
            var voucher = new Voucher { Id = 1, VoucherName = "OLDCODE", VoucherValue = "5", ExpirationDate = DateTime.UtcNow.AddDays(10) };
            var request = new UpdateVoucherRequest { Id = 1, Code = "VALIDCODE", DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
          
            _voucherRepositoryMock.Setup(x => x.FindAsync(request.Id)).ReturnsAsync(voucher);           

            var result = await _voucherService.UpdateVoucher(request);
            Assert.Equal(200, result.Code);
            Assert.Equal("Voucher updated successfully", result.Message);
        }
    }
}
