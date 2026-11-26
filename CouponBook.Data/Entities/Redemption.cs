namespace CouponBook.Data.Entities;
public class Redemption
{
    public int Id { get; set; }
    public int CustomerVoucherId { get; set; }
    public CustomerVoucher CustomerVoucher { get; set; } = null!;
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    public int ProviderUserId { get; set; }
    public ProviderUser ProviderUser { get; set; } = null!;
    public DateTime RedeemedAt { get; set; } = DateTime.UtcNow;
    public decimal FaceValueSnapshot { get; set; }
    public decimal? BillAmount { get; set; }
    public DateTime? ReversedAt { get; set; }
    public int? ReversedByUserId { get; set; }
    public string? ReversalReason { get; set; }
}
