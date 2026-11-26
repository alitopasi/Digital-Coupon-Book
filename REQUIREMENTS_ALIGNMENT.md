# BRD v2 alignment notes

This refactor treats the approved Irbid Offers BRD v2 as the source of truth.

Key corrections from the previous model:
- Merchant/Offer + many-to-many BookOffer was replaced by Provider + Voucher, where each voucher belongs to one book and one provider.
- Active duplicate-book ownership restriction was removed; repeat purchases are independent PurchasedBook rows.
- Book validity is now a duration (`ValidityDays`) and each PurchasedBook gets its own purchase-based expiry.
- CustomerVoucher stores snapshots and remaining uses; Redemption stores one confirmed use.
- Provider main/branch accounts, suspension and branch attribution were added.
- Gifts and promo codes were added.
- Customer authentication moved from email/password to mobile OTP flow.
- Provider quotas and sold-out calculations were added.
- Payment and redemption operations preserve idempotency/concurrency protection.

Still provider/configuration dependent:
- Production WhatsApp Business API transport/templates.
- Final Jordan-licensed payment gateway implementation and refund API.
- Push provider wiring and scheduled expiry reminders.
- Admin/reporting UI and React frontend are separate clients of this API.
