using FCSP.DTOs.Payment;
using FCSP.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Payment;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _paymentService.GetAllPayments();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var request = new GetPaymentByIdRequest { Id = id };
        var result = await _paymentService.GetPaymentById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPayment([FromBody] AddPaymentRequest request)
    {
        var result = await _paymentService.AddPayment(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentRequest request)
    {
        var result = await _paymentService.UpdatePayment(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var request = new DeletePaymentRequest { Id = id };
        var result = await _paymentService.DeletePayment(request);
        return Ok(result);
    }
} 