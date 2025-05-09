using FCSP.DTOs;
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


    [HttpGet("test-payos")]
    public async Task<IActionResult> TestPayOS()
    {
        var result = await _paymentService.TestPayOSAsync(new AddPaymentRequest { OrderId = 15, Amount = 4000 });
        return StatusCode(result.Code, result);
    }


    [HttpGet("get-payment-info/{paymentId}")]
    public async Task<IActionResult> GetPaymentInfo(long paymentId)
    {
        var request = new GetPaymentInfoRequest { PaymentId = paymentId };
        var result = await _paymentService.GetPaymentInfoFromPayOS(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _paymentService.GetAllPayments();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var request = new GetPaymentByIdRequest { Id = id };
        var result = await _paymentService.GetPaymentById(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPayment([FromBody] AddPaymentRequest request)
    {
        var result = await _paymentService.AddPayment(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentRequest request)
    {
        var result = await _paymentService.UpdatePayment(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> CancelPayment(long paymentId)
    {
        var request = new CancelPaymentRequest { PaymentId = paymentId };
        var result = await _paymentService.CancelPaymentFromPayOS(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost("confirm-webhook")]
    public async Task<IActionResult> ConfirmWebhook([FromBody] ConfirmWebhookRequest request)
    {
        var result = await _paymentService.ConfirmWebhook(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut("webhook")]
    public async Task<IActionResult> UpdatePaymentUsingWebhook([FromBody] UpdatePaymentUsingWebhookRequest request)
    {
        var result = await _paymentService.UpdatePaymentUsingWebhook(request);
        return StatusCode(result.Code, result);
    }
} 