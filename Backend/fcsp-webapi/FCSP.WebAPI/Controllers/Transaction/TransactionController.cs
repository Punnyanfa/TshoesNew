using FCSP.DTOs;
using FCSP.Services.TransactionService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Transaction;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("recharge")]
    public async Task<IActionResult> RechargeBalance([FromBody] RechargeRequestDTO request)
    {
        var result = await _transactionService.UpdateBalanceAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserTransactions(int userId)
    {
        var request = new TransactionHistoryRequestDTO { UserId = userId };
        var result = await _transactionService.GetUserTransactionsAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransactionById(int transactionId)
    {
        var request = new TransactionByIdRequestDTO { TransactionId = transactionId };
        var result = await _transactionService.GetTransactionByIdAsync(request);
        return StatusCode(result.Code, result);
    }
} 