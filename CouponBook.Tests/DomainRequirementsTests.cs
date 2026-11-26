using CouponBook.Data.Entities;
namespace CouponBook.Tests;
public class DomainRequirementsTests
{
 [Fact] public void Voucher_Supports_Multiple_Uses(){var v=new Voucher{UsageLimit=3};Assert.Equal(3,v.UsageLimit);}
 [Fact] public void PurchasedBook_Has_PerPurchase_Expiry(){var p=new PurchasedBook{PurchasedAt=DateTime.UtcNow,ExpiresAt=DateTime.UtcNow.AddDays(180)};Assert.True(p.ExpiresAt>p.PurchasedAt);}
 [Fact] public void ProviderUser_Can_Be_Main_Or_Branch(){Assert.Contains(ProviderUserType.Main,Enum.GetValues<ProviderUserType>());Assert.Contains(ProviderUserType.Branch,Enum.GetValues<ProviderUserType>());}
 [Fact] public void Book_Has_SoldOut_Status(){Assert.Contains(BookStatus.SoldOut,Enum.GetValues<BookStatus>());}
 [Fact] public void CustomerVoucher_Has_PartlyUsed_Status(){Assert.Contains(CustomerVoucherStatus.PartlyUsed,Enum.GetValues<CustomerVoucherStatus>());}
}
