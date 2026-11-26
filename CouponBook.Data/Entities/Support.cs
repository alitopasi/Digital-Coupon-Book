namespace CouponBook.Data.Entities;
public class OtpCode
{
    public int Id { get; set; }
    public string MobileNumber { get; set; } = "";
    public string CodeHash { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public int Attempts { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class WhatsAppMessage
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string MobileNumber { get; set; } = "";
    public string MessageType { get; set; } = "";
    public MessageDeliveryStatus Status { get; set; } = MessageDeliveryStatus.Pending;
    public string? ProviderReference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class DeviceToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Token { get; set; } = "";
    public string Platform { get; set; } = "web";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Type { get; set; } = "";
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string? DataJson { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class AuditLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string Action { get; set; } = "";
    public string EntityType { get; set; } = "";
    public string EntityId { get; set; } = "";
    public string Result { get; set; } = "";
    public string? Reason { get; set; }
    public string? MetadataJson { get; set; }
    public string? Ip { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
