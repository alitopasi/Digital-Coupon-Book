namespace CouponBook.Data.Entities;
public class Book
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string CoverUrl { get; set; } = "";
    public decimal Price { get; set; }
    public string Currency { get; set; } = "JOD";
    public int ValidityDays { get; set; }
    public DateTime SaleStartAt { get; set; }
    public DateTime SaleEndAt { get; set; }
    public BookStatus Status { get; set; } = BookStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Voucher> Vouchers { get; set; } = new();
}
