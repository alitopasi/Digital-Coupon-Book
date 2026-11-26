namespace CouponBook.Data.Entities;
public class Order
{
    public int Id { get; set; }
    public int BuyerUserId { get; set; }
    public User Buyer { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int? PromoCodeId { get; set; }
    public PromoCode? PromoCode { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "JOD";
    public string PaymentMethod { get; set; } = "card";
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public bool IsGift { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public List<Payment> Payments { get; set; } = new();
    public PurchasedBook? PurchasedBook { get; set; }
    public Gift? Gift { get; set; }
}
