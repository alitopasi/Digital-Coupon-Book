using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace CouponBook.Business.Sales;

public class PaymentOptions
{
    public string WebhookSecret { get; set; } = "";
}

public record PaymentSession(string ProviderTxnId, string PaymentUrl);

public record WebhookPayload(string ProviderTxnId, string Status, decimal Amount);

public class WebhookRejectedException : Exception
{
    public WebhookRejectedException(string message) : base(message) { }
}

// العقد: أي بوابة (وهمية أو حقيقية) لازم تعمل هدول بس
public interface IPaymentProvider
{
    string Name { get; }
    PaymentSession CreateSession(int orderId, decimal amount, string currency);
    WebhookPayload VerifyWebhook(string rawBody, string? signature);
}

public class MockPaymentProvider : IPaymentProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly PaymentOptions _options;

    public MockPaymentProvider(IOptions<PaymentOptions> options)
    {
        _options = options.Value;
    }

    public string Name => "mock";

    public PaymentSession CreateSession(int orderId, decimal amount, string currency)
    {
        var txnId = $"mock_{Guid.NewGuid():N}";
        return new PaymentSession(txnId, $"mock://pay/{txnId}");
    }

    // التوقيع: HMAC-SHA256 من نص الرسالة الخام + المفتاح السري
    public string Sign(string rawBody)
    {
        var key = Encoding.UTF8.GetBytes(_options.WebhookSecret);
        var hash = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(rawBody));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public WebhookPayload VerifyWebhook(string rawBody, string? signature)
    {
        var expected = Encoding.UTF8.GetBytes(Sign(rawBody));
        var actual = Encoding.UTF8.GetBytes(signature ?? "");
        if (!CryptographicOperations.FixedTimeEquals(expected, actual))
            throw new WebhookRejectedException("invalid_signature");

        WebhookPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<WebhookPayload>(rawBody, JsonOptions);
        }
        catch (JsonException)
        {
            throw new WebhookRejectedException("invalid_payload");
        }

        if (payload is null || string.IsNullOrWhiteSpace(payload.ProviderTxnId))
            throw new WebhookRejectedException("invalid_payload");

        return payload;
    }
}