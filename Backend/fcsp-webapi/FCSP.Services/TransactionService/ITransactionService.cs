using FCSP.DTOs;
using FCSP.Models.Entities;

namespace FCSP.Services.TransactionService;

public interface ITransactionService
{
    Task<BaseResponseModel<TransactionResponseDTO>> AddBalanceAsync(RechargeRequestDTO request);
    Task<BaseResponseModel<TransactionHistoryResponseDTO>> GetUserTransactionsAsync(TransactionHistoryRequestDTO request);
    Task<BaseResponseModel<TransactionResponseDTO>> GetTransactionByIdAsync(TransactionByIdRequestDTO request);
} 