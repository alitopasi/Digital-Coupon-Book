# Irbid Offers Backend

ASP.NET Core backend for Irbid Offers, aligned to Business Requirements Document v2.

## Stack
- ASP.NET Core / .NET 10
- Entity Framework Core
- PostgreSQL
- JWT access tokens after mobile/WhatsApp OTP verification

## Implemented domain
- Cities and categories
- Providers, branches and provider users
- Books and book-specific vouchers
- Provider quotas and sold-out calculation
- Orders and payment webhook idempotency
- Repeat purchase of the same book
- Per-purchase validity period
- Purchased-book and customer-voucher snapshots
- Single- and multi-use vouchers
- Atomic voucher redemption with branch/staff attribution
- Gifts and claim-on-login by recipient mobile
- Promo codes with total/per-customer limits and selected books
- Notifications, WhatsApp message log and audit log
- Customer/provider/admin role claims

## External integrations
Payment and WhatsApp transport are intentionally provider-agnostic. The current payment mock is for development. OTP requests are persisted/logged; connect an approved WhatsApp Business provider before production.

## Database migration
The old EF migrations represented the pre-v2 domain and were removed intentionally. After reviewing the connection string, create a fresh v2 migration:

```bash
dotnet ef migrations add V2RequirementsBaseline --project CouponBook.Data --startup-project CouponBook.Api
dotnet ef database update --project CouponBook.Data --startup-project CouponBook.Api
```

For an existing production database, do not run a destructive baseline directly. Create a data migration plan first.

## Important configuration
Keep secrets out of Git. Configure the PostgreSQL connection string, JWT signing key and payment webhook secret through environment variables or .NET user-secrets.
