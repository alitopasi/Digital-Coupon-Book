using System.Text;
using CouponBook.Business.Sales;
using Microsoft.AspNetCore.Mvc;

namespace CouponBook.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPurchaseService _purchases;

    public PaymentsController(IPurchaseService purchases)
    {
        _purchases = purchases;
    }

    // البوابة بتنادي هاد العنوان. ما في تسجيل دخول، الأمان بالتوقيع
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var raw = await reader.ReadToEndAsync();
        var signature = Request.Headers["X-Signature"].FirstOrDefault();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        var result = await _purchases.HandleWebhookAsync(raw, signature, ip);
        if (!result.Succeeded)
            return BadRequest(new { code = result.ErrorCode, errors = result.Errors });

        return Ok(new { result = result.Value!.Result });
    }
}