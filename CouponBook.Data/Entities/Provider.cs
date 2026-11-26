namespace CouponBook.Data.Entities;
public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string LogoUrl { get; set; } = "";
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string ContactPerson { get; set; } = "";
    public string Phone { get; set; } = "";
    public string WorkingHours { get; set; } = "";
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Branch> Branches { get; set; } = new();
    public List<Voucher> Vouchers { get; set; } = new();
}

public class ProviderUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public ProviderUserType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
}
