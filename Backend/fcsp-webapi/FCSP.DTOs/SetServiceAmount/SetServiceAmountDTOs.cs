namespace FCSP.DTOs.SetServiceAmount
{
    public class GetSetServiceAmountRequest
    {
        public long Id { get; set; }
    }

    public class GetSetServiceAmountResponse
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddSetServiceAmountRequest
    {
        public long ServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class AddSetServiceAmountResponse
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateSetServiceAmountRequest
    {
        public long Id { get; set; }
        public DateTime? EndDate { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
    }

    public class UpdateSetServiceAmountResponse
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 