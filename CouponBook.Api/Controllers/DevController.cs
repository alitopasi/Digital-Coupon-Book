using System.Text.Json;
using CouponBook.Api.Dtos;
using CouponBook.Business.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponBook.Api.Controllers;

[ApiController]
[Route("api/dev")]
[Authorize(Roles = "Customer")]
public class DevController : ControllerBase
{
    private readonly IPurchaseService _purchases;
    private readonly MockPaymentProvider _mock;
    private readonly IWebHostEnvironment _env;

    public DevController(IPurchaseService purchases, MockPaymentProvider mock, IWebHostEnvironment env)
    {
        _purchases = purchases;
        _mock = mock;
        _env = env;
    }

    // بيمثّل البوابة: بيبني webhook موقّع وبيمرره على نفس الدالة الحقيقية
    [HttpPost("mock-pay")]
    public async Task<IActionResult> MockPay(MockPayRequest request)
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var userId = int.Parse(User.FindFirst("sub")!.Value);
        var payment = await _purchases.GetOwnPaymentAsync(userId, request.PaymentId);
        if (payment is null)
            return NotFound();

        var raw = JsonSerializer.Serialize(new
        {
            providerTxnId = payment.ProviderTxnId,
            status = request.Outcome,
            amount = payment.Amount
        });
        var signature = _mock.Sign(raw);
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        var result = await _purchases.HandleWebhookAsync(raw, signature, ip);
        if (!result.Succeeded)
            return BadRequest(new { code = result.ErrorCode, errors = result.Errors });

        return Ok(new { result = result.Value!.Result, orderStatus = result.Value.Status });
    }
}