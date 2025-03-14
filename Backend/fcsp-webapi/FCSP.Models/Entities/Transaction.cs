namespace FCSP.Models.Entities;

public class Transaction : BaseEntity
{
    public long ReceiverId { get; set; }
    public long OrderDetailId { get; set; }
    public long PaymentId { get; set; }
    public float Amount { get; set; } // Note: Changed from bool to float based on schema context

    // Navigation properties
    public virtual User Receiver { get; set; } = null!;
    public virtual OrderDetail OrderDetail { get; set; } = null!;
    public virtual Payment Payment { get; set; } = null!;
} 