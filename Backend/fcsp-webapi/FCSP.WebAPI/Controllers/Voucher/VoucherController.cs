using FCSP.DTOs;
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
            var result = await _voucherService.GetAllVouchers();
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(long id)
        {
            var request = new GetVoucherByIdRequest { Id = id };
            var result = await _voucherService.GetVoucherById(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetVoucherByOrderId(long orderId)
        {
            var request = new GetVoucherByOrderIdRequest { OrderId = orderId };
            var result = await _voucherService.GetVoucherByOrderId(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher([FromBody] AddVoucherRequest request)
        {
            var result = await _voucherService.AddVoucher(request);
            return StatusCode(result.Code, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(long id, [FromBody] UpdateVoucherRequest request)
        {
            if (id != request.Id)
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "ID in URL does not match ID in request body" });

            var result = await _voucherService.UpdateVoucher(request);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(long id)
        {
            var request = new DeleteVoucherRequest { Id = id };
            var result = await _voucherService.DeleteVoucher(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost("update-expired")]
        public async Task<IActionResult> UpdateExpiredVouchers()
        {
            var result = await _voucherService.UpdateExpiredVouchers();
            return StatusCode(result.Code, result);
        }

        [HttpGet("non-expired")]
        public async Task<IActionResult> GetNonExpiredVouchers()
        {
            var result = await _voucherService.GetNonExpiredVouchers();
            return StatusCode(result.Code, result);
        }
    }
}