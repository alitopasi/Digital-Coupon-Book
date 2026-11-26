namespace CouponBook.Data.Entities;
public class PurchasedBook
{
    public int Id { get; set; }
    public int? OwnerUserId { get; set; }
    public User? Owner { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int? GiftId { get; set; }
    public Gift? Gift { get; set; }
    public DateTime PurchasedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public PurchasedBookStatus Status { get; set; } = PurchasedBookStatus.Active;
    public string BookTitleSnapshot { get; set; } = "";
    public decimal PriceSnapshot { get; set; }
    public List<CustomerVoucher> Vouchers { get; set; } = new();
}
