using CouponBook.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CouponBook.Data;

public class AppDbContext : IdentityUserContext<User, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<City> Cities => Set<City>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<ProviderUser> ProviderUsers => Set<ProviderUser>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PurchasedBook> PurchasedBooks => Set<PurchasedBook>();
    public DbSet<CustomerVoucher> CustomerVouchers => Set<CustomerVoucher>();
    public DbSet<Redemption> Redemptions => Set<Redemption>();
    public DbSet<Gift> Gifts => Set<Gift>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<PromoCodeBook> PromoCodeBooks => Set<PromoCodeBook>();
    public DbSet<PromoCodeUsage> PromoCodeUsages => Set<PromoCodeUsage>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
    public DbSet<WhatsAppMessage> WhatsAppMessages => Set<WhatsAppMessage>();
    public DbSet<DeviceToken> DeviceTokens => Set<DeviceToken>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        base.OnModelCreating(m);
        m.Entity<User>(e => { e.ToTable("Users"); e.Property(x=>x.FullName).HasMaxLength(150); e.Property(x=>x.Language).HasMaxLength(5); e.HasIndex(x=>x.PhoneNumber).IsUnique(); });
        m.Entity<Category>().Property(x=>x.NameEn).HasMaxLength(100);
        m.Entity<Provider>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.HasOne(x=>x.Category).WithMany().HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<Branch>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.Property(x=>x.Latitude).HasPrecision(9,6); e.Property(x=>x.Longitude).HasPrecision(9,6); e.HasOne(x=>x.Provider).WithMany(x=>x.Branches).HasForeignKey(x=>x.ProviderId); });
        m.Entity<ProviderUser>(e => { e.Property(x=>x.Type).HasConversion<string>(); e.Property(x=>x.Status).HasConversion<string>(); e.HasIndex(x=>new{x.UserId,x.ProviderId,x.BranchId}).IsUnique(); e.HasOne(x=>x.User).WithMany(x=>x.ProviderAccounts).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Provider).WithMany().HasForeignKey(x=>x.ProviderId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Branch).WithMany().HasForeignKey(x=>x.BranchId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<Book>(e => { e.Property(x=>x.Price).HasPrecision(10,3); e.Property(x=>x.Status).HasConversion<string>(); e.HasOne(x=>x.City).WithMany().HasForeignKey(x=>x.CityId).OnDelete(DeleteBehavior.Restrict); e.ToTable(t=>{t.HasCheckConstraint("book_price_nonnegative","\"Price\" >= 0");t.HasCheckConstraint("book_validity_positive","\"ValidityDays\" > 0");t.HasCheckConstraint("book_sale_dates","\"SaleEndAt\" >= \"SaleStartAt\"");}); });
        m.Entity<Voucher>(e => { e.Property(x=>x.DiscountType).HasConversion<string>(); e.Property(x=>x.FaceValue).HasPrecision(10,3); e.Property(x=>x.DiscountValue).HasPrecision(10,3); e.HasOne(x=>x.Book).WithMany(x=>x.Vouchers).HasForeignKey(x=>x.BookId); e.HasOne(x=>x.Provider).WithMany(x=>x.Vouchers).HasForeignKey(x=>x.ProviderId).OnDelete(DeleteBehavior.Restrict); e.ToTable(t=>{t.HasCheckConstraint("voucher_usage_positive","\"UsageLimit\" >= 1");t.HasCheckConstraint("voucher_quota_nonnegative","\"ProviderQuota\" >= 0");}); });
        m.Entity<Order>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.Property(x=>x.Subtotal).HasPrecision(10,3); e.Property(x=>x.DiscountAmount).HasPrecision(10,3); e.Property(x=>x.Amount).HasPrecision(10,3); e.HasOne(x=>x.Buyer).WithMany().HasForeignKey(x=>x.BuyerUserId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Book).WithMany().HasForeignKey(x=>x.BookId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<Payment>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.Property(x=>x.Amount).HasPrecision(10,3); e.HasIndex(x=>new{x.Provider,x.ProviderTxnId}).IsUnique(); e.HasIndex(x=>x.IdempotencyKey).IsUnique(); });
        m.Entity<PurchasedBook>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.Property(x=>x.PriceSnapshot).HasPrecision(10,3); e.HasOne(x=>x.Order).WithOne(x=>x.PurchasedBook).HasForeignKey<PurchasedBook>(x=>x.OrderId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Owner).WithMany().HasForeignKey(x=>x.OwnerUserId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<CustomerVoucher>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.Property(x=>x.FaceValueSnapshot).HasPrecision(10,3); e.HasIndex(x=>x.Code).IsUnique(); e.HasIndex(x=>new{x.PurchasedBookId,x.VoucherId}).IsUnique(); e.ToTable(t=>t.HasCheckConstraint("customer_voucher_uses_nonnegative","\"UsesRemaining\" >= 0")); });
        m.Entity<Redemption>(e => { e.Property(x=>x.FaceValueSnapshot).HasPrecision(10,3); e.Property(x=>x.BillAmount).HasPrecision(10,3); e.HasOne(x=>x.CustomerVoucher).WithMany(x=>x.Redemptions).HasForeignKey(x=>x.CustomerVoucherId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Branch).WithMany().HasForeignKey(x=>x.BranchId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.ProviderUser).WithMany().HasForeignKey(x=>x.ProviderUserId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<Gift>(e => { e.Property(x=>x.Status).HasConversion<string>(); e.HasIndex(x=>x.OrderId).IsUnique(); e.HasOne(x=>x.Order).WithOne(x=>x.Gift).HasForeignKey<Gift>(x=>x.OrderId); e.HasOne(x=>x.Sender).WithMany().HasForeignKey(x=>x.SenderUserId).OnDelete(DeleteBehavior.Restrict); e.HasOne(x=>x.Recipient).WithMany().HasForeignKey(x=>x.RecipientUserId).OnDelete(DeleteBehavior.Restrict); });
        m.Entity<PromoCode>(e => { e.Property(x=>x.DiscountType).HasConversion<string>(); e.Property(x=>x.DiscountValue).HasPrecision(10,3); e.HasIndex(x=>x.Code).IsUnique(); });
        m.Entity<PromoCodeBook>(e => { e.HasKey(x=>new{x.PromoCodeId,x.BookId}); e.HasOne(x=>x.PromoCode).WithMany(x=>x.Books).HasForeignKey(x=>x.PromoCodeId); e.HasOne(x=>x.Book).WithMany().HasForeignKey(x=>x.BookId); });
        m.Entity<PromoCodeUsage>(e => { e.Property(x=>x.DiscountAmount).HasPrecision(10,3); e.HasIndex(x=>x.OrderId).IsUnique(); });
        m.Entity<OtpCode>(e => { e.HasIndex(x=>new{x.MobileNumber,x.CreatedAt}); });
        m.Entity<WhatsAppMessage>().Property(x=>x.Status).HasConversion<string>();
        m.Entity<DeviceToken>().HasIndex(x=>x.Token).IsUnique();
        m.Entity<AuditLog>(e=>{ e.Property(x=>x.Action).HasMaxLength(60); e.Property(x=>x.EntityType).HasMaxLength(60); });
    }
}
