using FCSP.DTOs.PaymentGateway;
using FCSP.Services.PaymentGatewayService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.PaymentGateway;

[Route("api/[controller]")]
[ApiController]
public class PaymentGatewayController : ControllerBase
{
    private readonly IPaymentGatewayService _paymentGatewayService;

    public PaymentGatewayController(IPaymentGatewayService paymentGatewayService)
    {
        _paymentGatewayService = paymentGatewayService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPaymentGateways()
    {
        var result = await _paymentGatewayService.GetAllPaymentGateways();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentGatewayById(int id)
    {
        var request = new GetPaymentGatewayByIdRequest { Id = id };
        var result = await _paymentGatewayService.GetPaymentGatewayById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPaymentGateway([FromBody] AddPaymentGatewayRequest request)
    {
        var result = await _paymentGatewayService.AddPaymentGateway(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePaymentGateway([FromBody] UpdatePaymentGatewayRequest request)
    {
        var result = await _paymentGatewayService.UpdatePaymentGateway(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaymentGateway(int id)
    {
        var request = new DeletePaymentGatewayRequest { Id = id };
        var result = await _paymentGatewayService.DeletePaymentGateway(request);
        return Ok(result);
    }
} 