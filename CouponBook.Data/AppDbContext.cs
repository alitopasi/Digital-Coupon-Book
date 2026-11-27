using CouponBook.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CouponBook.Data;

public class AppDbContext : IdentityUserContext<User, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

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

        m.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.FullName).HasMaxLength(150);
            entity.Property(x => x.Language).HasMaxLength(5);
            entity.HasIndex(x => x.PhoneNumber).IsUnique();
        });

        m.Entity<Category>(entity =>
        {
            entity.Property(x => x.NameEn).HasMaxLength(100);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
        });

        m.Entity<Provider>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();

            entity.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<Branch>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.Property(x => x.Latitude).HasPrecision(9, 6);
            entity.Property(x => x.Longitude).HasPrecision(9, 6);

            entity.HasOne(x => x.Provider)
                .WithMany(x => x.Branches)
                .HasForeignKey(x => x.ProviderId);
        });

        m.Entity<ProviderUser>(entity =>
        {
            entity.Ignore("Type");

            entity.Property(x => x.Status).HasConversion<string>();
            entity.HasIndex(x => x.UserId).IsUnique();

            entity.HasOne(x => x.User)
                .WithMany(x => x.ProviderAccounts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Provider)
                .WithMany()
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => new { x.ProviderId, x.BranchId })
                .HasPrincipalKey(x => new { x.ProviderId, x.Id })
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<Book>(entity =>
        {
            entity.Property(x => x.Price).HasPrecision(10, 3);
            entity.Property(x => x.Status).HasConversion<string>();

            entity.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "book_price_nonnegative", "\"Price\" >= 0");

                table.HasCheckConstraint(
                    "book_validity_positive", "\"ValidityDays\" > 0");

                table.HasCheckConstraint(
                    "book_sale_dates", "\"SaleEndAt\" >= \"SaleStartAt\"");
            });
        });

        m.Entity<Voucher>(entity =>
        {
            entity.Property(x => x.DiscountType).HasConversion<string>();
            entity.Property(x => x.FaceValue).HasPrecision(10, 3);
            entity.Property(x => x.DiscountValue).HasPrecision(10, 3);

            entity.HasOne(x => x.Book)
                .WithMany(x => x.Vouchers)
                .HasForeignKey(x => x.BookId);

            entity.HasOne(x => x.Provider)
                .WithMany(x => x.Vouchers)
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "voucher_usage_positive", "\"UsageLimit\" >= 1");

                table.HasCheckConstraint(
                    "voucher_quota_nonnegative", "\"ProviderQuota\" >= 0");
            });
        });

        m.Entity<Order>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.Property(x => x.Subtotal).HasPrecision(10, 3);
            entity.Property(x => x.DiscountAmount).HasPrecision(10, 3);
            entity.Property(x => x.Amount).HasPrecision(10, 3);

            entity.HasOne(x => x.Buyer)
                .WithMany()
                .HasForeignKey(x => x.BuyerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<Payment>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.Property(x => x.Amount).HasPrecision(10, 3);
            entity.HasIndex(x => new { x.Provider, x.ProviderTxnId }).IsUnique();
            entity.HasIndex(x => x.IdempotencyKey).IsUnique();
        });

        m.Entity<PurchasedBook>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.Property(x => x.PriceSnapshot).HasPrecision(10, 3);

            entity.HasOne(x => x.Order)
                .WithOne(x => x.PurchasedBook)
                .HasForeignKey<PurchasedBook>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<CustomerVoucher>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.Property(x => x.FaceValueSnapshot).HasPrecision(10, 3);
            entity.HasIndex(x => x.Code).IsUnique();
            entity.HasIndex(x => new { x.PurchasedBookId, x.VoucherId }).IsUnique();

            entity.ToTable(table => table.HasCheckConstraint(
                "customer_voucher_uses_nonnegative", "\"UsesRemaining\" >= 0"));
        });

        m.Entity<Redemption>(entity =>
        {
            entity.Property(x => x.FaceValueSnapshot).HasPrecision(10, 3);
            entity.Property(x => x.BillAmount).HasPrecision(10, 3);

            entity.HasOne(x => x.CustomerVoucher)
                .WithMany(x => x.Redemptions)
                .HasForeignKey(x => x.CustomerVoucherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ProviderUser)
                .WithMany()
                .HasForeignKey(x => x.ProviderUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<Gift>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<string>();
            entity.HasIndex(x => x.OrderId).IsUnique();

            entity.HasOne(x => x.Order)
                .WithOne(x => x.Gift)
                .HasForeignKey<Gift>(x => x.OrderId);

            entity.HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Recipient)
                .WithMany()
                .HasForeignKey(x => x.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<PromoCode>(entity =>
        {
            entity.Property(x => x.DiscountType).HasConversion<string>();
            entity.Property(x => x.DiscountValue).HasPrecision(10, 3);
            entity.HasIndex(x => x.Code).IsUnique();
        });

        m.Entity<PromoCodeBook>(entity =>
        {
            entity.HasKey(x => new { x.PromoCodeId, x.BookId });

            entity.HasOne(x => x.PromoCode)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.PromoCodeId);

            entity.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId);
        });

        m.Entity<PromoCodeUsage>(entity =>
        {
            entity.Property(x => x.DiscountAmount).HasPrecision(10, 3);
            entity.HasIndex(x => x.OrderId).IsUnique();
        });

        m.Entity<OtpCode>()
            .HasIndex(x => new { x.MobileNumber, x.CreatedAt });

        m.Entity<WhatsAppMessage>()
            .Property(x => x.Status)
            .HasConversion<string>();

        m.Entity<DeviceToken>()
            .HasIndex(x => x.Token)
            .IsUnique();

        m.Entity<AuditLog>(entity =>
        {
            entity.Property(x => x.Action).HasMaxLength(60);
            entity.Property(x => x.EntityType).HasMaxLength(60);
        });
    }
}