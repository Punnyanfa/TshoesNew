using FCSP.DTOs.Voucher;
using FCSP.Services.VoucherService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Voucher
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVouchers()
        {
            var vouchers = await _voucherService.GetAllVouchers();
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(long id)
        {
            var request = new GetVoucherByIdRequest { Id = id };
            var voucher = await _voucherService.GetVoucherById(request);
            return Ok(voucher);
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetVoucherByOrderId(long orderId)
        {
            var request = new GetVoucherByOrderIdRequest { OrderId = orderId };
            var voucher = await _voucherService.GetVoucherByOrderId(request);
            return Ok(voucher);
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher([FromBody] AddVoucherRequest request)
        {
            var response = await _voucherService.AddVoucher(request);
            return CreatedAtAction(nameof(GetVoucherById), new { id = response.VoucherId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(long id, [FromBody] UpdateVoucherRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _voucherService.UpdateVoucher(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(long id)
        {
            var request = new DeleteVoucherRequest { Id = id };
            var response = await _voucherService.DeleteVoucher(request);
            return Ok(response);
        }

        [HttpPost("update-expired")]
        public async Task<IActionResult> UpdateExpiredVouchers()
        {
            var updatedCount = await _voucherService.UpdateExpiredVouchers();
            return Ok(new { UpdatedCount = updatedCount });
        }
    }
}