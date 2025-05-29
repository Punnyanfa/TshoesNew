using FCSP.Repositories.Interfaces;
using FCSP.Services.VoucherService;
using Microsoft.Extensions.Configuration;
using Moq;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;

namespace FCSP.Tests
{
    public class VoucherServiceAddTests
    {
        private readonly Mock<IVoucherRepository> _voucherRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly VoucherService _voucherService;

        public VoucherServiceAddTests()
        {
            _voucherRepositoryMock = new Mock<IVoucherRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _voucherService = new VoucherService(_voucherRepositoryMock.Object, _configurationMock.Object);
        }
        [Fact]
        public async Task VoucherServiceAdd_CodeIsNull()
        {

            var request = new AddVoucherRequest { Code = null, DiscountAmount = 10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            _voucherRepositoryMock.Setup(x => x.FindAsync(null)).ReturnsAsync((Voucher)null);

            var result = await _voucherService.AddVoucher(request);

            Assert.Equal(400, result.Code);
            Assert.Equal("Code cannot be null or empty", result.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_CodeLessThanFive()
        {
            var request = new AddVoucherRequest { Code = "LCD", DiscountAmount = 20, ExpiryDate = DateTime.UtcNow.AddDays(30) };

            var exception = await _voucherService.AddVoucher(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Code must be at least 5 characters", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_CodeGreateThanTwenty()
        {
            var longString = new string('A', 21);
            var request = new AddVoucherRequest { Code = longString, DiscountAmount = 20, ExpiryDate = DateTime.UtcNow.AddDays(30) };

            var exception = await _voucherService.AddVoucher(request);

            Assert.Equal(400, exception.Code);
            Assert.Equal("Code must be greater than 20 characters", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_InvalidFormatCode()
        {
            var request = new AddVoucherRequest { Code = "12345@#$", DiscountAmount = 20, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            var exception = await _voucherService.AddVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Code should not include special character", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_DiscountAmountIsNull()
        {
            var request = new AddVoucherRequest { Code = "VALIDCODE", DiscountAmount = 0, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            var exception = await _voucherService.AddVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be greater than 0", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_DiscountAmountIsNegative()
        {
            var request = new AddVoucherRequest { Code = "VALIDCODE", DiscountAmount = -10, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            var exception = await _voucherService.AddVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be greater than 0", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_DiscountAmountGreaterThanThrirty()
        {
            var request = new AddVoucherRequest { Code = "VALIDCODE", DiscountAmount = 35, ExpiryDate = DateTime.UtcNow.AddDays(30) };
            var exception = await _voucherService.AddVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("Discount amount must be less than or equal to 30", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_ExpiryDateIsInvalid()
        {
            var dateInPast = DateTime.UtcNow.AddDays(-10);
            var request = new AddVoucherRequest { Code = "VALIDCODE", DiscountAmount = 20, ExpiryDate = dateInPast };
            var exception = await _voucherService.AddVoucher(request);
            Assert.Equal(400, exception.Code);
            Assert.Equal("expiryDate can not be the past ", exception.Message);
        }
        [Fact]
        public async Task VoucherServiceAdd_ValidRequest()
        {
            var request = new AddVoucherRequest
            {
                Code = "VALIDCODE",
                DiscountAmount = 20,
                ExpiryDate = DateTime.UtcNow.AddDays(30)
            };
            var addedVoucher = new Voucher
            {
                Id = 1,
                VoucherName = request.Code,
                ExpirationDate = request.ExpiryDate,
                CreatedAt = DateTime.UtcNow
            };
            _voucherRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Voucher>())).ReturnsAsync(addedVoucher);
            var result = await _voucherService.AddVoucher(request);
            Assert.Equal(200, result.Code);
            Assert.Equal("Voucher created successfully", result.Message);

        }
    }
}
