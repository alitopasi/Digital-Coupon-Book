namespace CouponBook.Data.Entities;
public class CustomerVoucher
{
    public int Id { get; set; }
    public int PurchasedBookId { get; set; }
    public PurchasedBook PurchasedBook { get; set; } = null!;
    public int VoucherId { get; set; }
    public Voucher Voucher { get; set; } = null!;
    public string Code { get; set; } = "";
    public int UsageLimitSnapshot { get; set; }
    public int UsesRemaining { get; set; }
    public decimal FaceValueSnapshot { get; set; }
    public string TitleSnapshot { get; set; } = "";
    public string TermsSnapshot { get; set; } = "";
    public CustomerVoucherStatus Status { get; set; } = CustomerVoucherStatus.Available;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Redemption> Redemptions { get; set; } = new();
}
