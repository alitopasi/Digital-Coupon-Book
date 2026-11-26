namespace CouponBook.Data.Entities;

public enum AccountStatus { Active, Suspended }
public enum ProviderUserType { Main, Branch }
public enum DiscountType { Percentage, FixedAmount, FreeItem, BuyOneGetOne }
public enum BookStatus { Draft, Published, SoldOut, Unpublished }
public enum OrderStatus { Pending, Paid, Failed, Cancelled, Refunded }
public enum PaymentStatus { Pending, Paid, Failed, Cancelled, Refunded }
public enum PurchasedBookStatus { PendingPayment, Active, Expired, Refunded }
public enum CustomerVoucherStatus { Available, PartlyUsed, Used, Expired }
public enum GiftStatus { Sent, Claimed }
public enum PromoDiscountType { Percentage, FixedAmount }
public enum MessageDeliveryStatus { Pending, Sent, Delivered, Failed }
