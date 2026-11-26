namespace CouponBook.Data.Entities;
public class Voucher
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DiscountType DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal FaceValue { get; set; }
    public int UsageLimit { get; set; } = 1;
    public int ProviderQuota { get; set; }
    public string Terms { get; set; } = "";
    public int DisplayOrder { get; set; }
}
