namespace CouponBook.Data.Entities;
public class Gift
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int SenderUserId { get; set; }
    public User Sender { get; set; } = null!;
    public int? RecipientUserId { get; set; }
    public User? Recipient { get; set; }
    public string RecipientMobile { get; set; } = "";
    public string? Message { get; set; }
    public GiftStatus Status { get; set; } = GiftStatus.Sent;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClaimedAt { get; set; }
}
public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public PromoDiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int TotalUsageLimit { get; set; }
    public int PerCustomerLimit { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public List<PromoCodeBook> Books { get; set; } = new();
}
public class PromoCodeBook
{
    public int PromoCodeId { get; set; }
    public PromoCode PromoCode { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
public class PromoCodeUsage
{
    public int Id { get; set; }
    public int PromoCodeId { get; set; }
    public PromoCode PromoCode { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public decimal DiscountAmount { get; set; }
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;
}
