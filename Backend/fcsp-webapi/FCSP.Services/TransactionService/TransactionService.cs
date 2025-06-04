using FCSP.DTOs;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Services.TransactionService;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
    }

    public async Task<BaseResponseModel<TransactionResponseDTO>> UpdateBalanceAsync(RechargeRequestDTO request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<TransactionResponseDTO>
                {
                    Code = 404,
                    Message = "User not found"
                };
            }

            // Create a new transaction for the recharge
            var transaction = new Transaction
            {
                ReceiverId = request.UserId,
                OrderDetailId = 1,
                PaymentId = request.PaymentId.Value,
                Amount = (int)request.Amount,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Update user balance
            user.Balance = (user.Balance ?? 0) + request.Amount;

            await _transactionRepository.AddAsync(transaction);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<TransactionResponseDTO>
            {
                Code = 200,
                Message = "Balance recharged successfully",
                Data = new TransactionResponseDTO
                {
                    Id = transaction.Id,
                    ReceiverId = transaction.ReceiverId,
                    ReceiverName = user.Name,
                    Amount = transaction.Amount,
                    CreatedAt = transaction.CreatedAt
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<TransactionResponseDTO>
            {
                Code = 500,
                Message = $"Error recharging balance: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponseModel<TransactionHistoryResponseDTO>> GetUserTransactionsAsync(TransactionHistoryRequestDTO request)
    {
        try
        {
            var transactions = await _transactionRepository.GetAll()
                .Where(t => t.ReceiverId == request.UserId && !t.IsDeleted)
                .Include(t => t.Receiver)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionResponseDTO
                {
                    Id = t.Id,
                    ReceiverId = t.ReceiverId,
                    ReceiverName = t.Receiver.Name,
                    Amount = t.Amount,
                    CreatedAt = t.CreatedAt,
                    OrderDetailId = t.OrderDetailId.ToString(),
                    PaymentId = t.PaymentId.ToString()
                })
                .ToListAsync();

            return new BaseResponseModel<TransactionHistoryResponseDTO>
            {
                Code = 200,
                Message = "Transactions retrieved successfully",
                Data = new TransactionHistoryResponseDTO
                {
                    Transactions = transactions
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<TransactionHistoryResponseDTO>
            {
                Code = 500,
                Message = $"Error retrieving transactions: {ex.Message}"
            };
        }
    }

    public async Task<BaseResponseModel<TransactionResponseDTO>> GetTransactionByIdAsync(TransactionByIdRequestDTO request)
    {
        try
        {
            var transaction = await _transactionRepository.GetAll()
                .Include(t => t.Receiver)
                .FirstOrDefaultAsync(t => t.Id == request.TransactionId && !t.IsDeleted);

            if (transaction == null)
            {
                return new BaseResponseModel<TransactionResponseDTO>
                {
                    Code = 404,
                    Message = "Transaction not found"
                };
            }

            return new BaseResponseModel<TransactionResponseDTO>
            {
                Code = 200,
                Message = "Transaction retrieved successfully",
                Data = new TransactionResponseDTO
                {
                    Id = transaction.Id,
                    ReceiverId = transaction.ReceiverId,
                    ReceiverName = transaction.Receiver.Name,
                    Amount = transaction.Amount,
                    CreatedAt = transaction.CreatedAt,
                    OrderDetailId = transaction.OrderDetailId.ToString(),
                    PaymentId = transaction.PaymentId.ToString()
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<TransactionResponseDTO>
            {
                Code = 500,
                Message = $"Error retrieving transaction: {ex.Message}"
            };
        }
    }
} 