using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs;

public class RechargeRequestDTO
{
    public long UserId { get; set; }
    public long? PaymentId { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public float Amount { get; set; }
}

public class TransactionResponseDTO
{
    public long Id { get; set; }
    public long ReceiverId { get; set; }
    public string ReceiverName { get; set; } = null!;
    public float Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? OrderDetailId { get; set; }
    public string? PaymentId { get; set; }
}

public class TransactionHistoryResponseDTO
{
    public List<TransactionResponseDTO> Transactions { get; set; } = [];
}

public class TransactionHistoryRequestDTO
{
    public long UserId { get; set; }
}

public class TransactionByIdRequestDTO
{
    public long TransactionId { get; set; }
} 