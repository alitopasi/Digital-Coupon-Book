using Microsoft.AspNetCore.Identity;
namespace CouponBook.Data.Entities;

public class User : IdentityUser<int>
{
    public string FullName { get; set; } = "";
    public string Language { get; set; } = "ar";
    public bool IsCustomer { get; set; } = true;
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ProviderUser> ProviderAccounts { get; set; } = new();
}
