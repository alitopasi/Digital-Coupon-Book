namespace CouponBook.Data.Entities;
public class Branch
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Phone { get; set; } = "";
    public string WorkingHours { get; set; } = "";
    public AccountStatus Status { get; set; } = AccountStatus.Active;
}
