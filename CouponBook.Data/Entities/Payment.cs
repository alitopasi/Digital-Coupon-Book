namespace CouponBook.Data.Entities;
public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public string Provider { get; set; } = "";
    public string? ProviderTxnId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = "card";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string IdempotencyKey { get; set; } = "";
    public string? Last4 { get; set; }
    public string? RawResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public DateTime? RefundedAt { get; set; }
}
