using System.ComponentModel.DataAnnotations;
using CouponBook.Data;
using CouponBook.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CouponBook.Api.Controllers;

public record CreateBookRequest(
    [Range(1, int.MaxValue)] int CityId,
    [Required, StringLength(150)] string Title,
    [Required, StringLength(2000)] string Description,
    decimal Price,
    [Range(1, int.MaxValue)] int ValidityDays,
    DateTimeOffset SaleStartAt,
    DateTimeOffset SaleEndAt);

public record CreateBookVoucherRequest(
    [Range(1, int.MaxValue)] int ProviderId,
    [Required, StringLength(150)] string Title,
    [Required, StringLength(2000)] string Description,
    DiscountType DiscountType,
    decimal? DiscountValue,
    decimal FaceValue,
    [Range(1, int.MaxValue)] int UsageLimit,
    [Range(1, int.MaxValue)] int ProviderQuota,
    [Required, StringLength(2000)] string Terms,
    [Range(0, int.MaxValue)] int DisplayOrder);

[ApiController]
[Route("api/admin/books")]
[Authorize(Roles = "Admin")]
public class AdminBooksController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminBooksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(
        CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        if (!IsPositiveAmount(request.Price))
        {
            return BadRequest(new { code = "invalid_price" });
        }

        if (request.SaleStartAt == default ||
            request.SaleEndAt <= request.SaleStartAt)
        {
            return BadRequest(new { code = "invalid_sale_dates" });
        }

        var cityExists = await _db.Cities.AnyAsync(
            city => city.Id == request.CityId && city.IsActive,
            cancellationToken);

        if (!cityExists)
        {
            return NotFound(new { code = "active_city_not_found" });
        }

        var book = new Book
        {
            CityId = request.CityId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            Currency = "JOD",
            ValidityDays = request.ValidityDays,
            SaleStartAt = request.SaleStartAt.UtcDateTime,
            SaleEndAt = request.SaleEndAt.UtcDateTime,
            Status = BookStatus.Draft
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync(cancellationToken);

        return StatusCode(201, new { id = book.Id, status = book.Status });
    }

    [HttpPost("{bookId:int}/vouchers")]
    public async Task<IActionResult> AddVoucher(
        int bookId,
        CreateBookVoucherRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        if (!IsPositiveAmount(request.FaceValue))
        {
            return BadRequest(new { code = "invalid_face_value" });
        }

        var validDiscount = request.DiscountType switch
        {
            DiscountType.Percentage =>
                request.DiscountValue is decimal percentage &&
                percentage <= 100 &&
                IsPositiveAmount(percentage),

            DiscountType.FixedAmount =>
                request.DiscountValue is decimal amount &&
                IsPositiveAmount(amount),

            DiscountType.FreeItem or DiscountType.BuyOneGetOne =>
                request.DiscountValue is null,

            _ => false
        };

        if (!validDiscount)
        {
            return BadRequest(new { code = "invalid_discount" });
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        var book = await LockBookAsync(bookId, cancellationToken);
        if (book is null)
        {
            return NotFound(new { code = "book_not_found" });
        }

        if (book.Status != BookStatus.Draft)
        {
            return Conflict(new { code = "book_must_be_draft" });
        }

        var providerExists = await _db.Providers.AnyAsync(
            provider => provider.Id == request.ProviderId &&
                        provider.Status == AccountStatus.Active,
            cancellationToken);

        if (!providerExists)
        {
            return NotFound(new { code = "active_provider_not_found" });
        }

        var voucher = new Voucher
        {
            BookId = book.Id,
            ProviderId = request.ProviderId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            FaceValue = request.FaceValue,
            UsageLimit = request.UsageLimit,
            ProviderQuota = request.ProviderQuota,
            Terms = request.Terms.Trim(),
            DisplayOrder = request.DisplayOrder
        };

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return StatusCode(201, new { id = voucher.Id, bookId = book.Id });
    }

    [HttpPut("{bookId:int}/publish")]
    public async Task<IActionResult> PublishBook(
        int bookId,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        var book = await LockBookAsync(bookId, cancellationToken);
        if (book is null)
        {
            return NotFound(new { code = "book_not_found" });
        }

        if (book.Status == BookStatus.Published)
        {
            return Ok(new { id = book.Id, status = book.Status });
        }

        if (book.Status != BookStatus.Draft)
        {
            return Conflict(new { code = "book_must_be_draft" });
        }

        if (book.SaleEndAt <= DateTime.UtcNow)
        {
            return Conflict(new { code = "sale_period_ended" });
        }

        var hasVoucher = await _db.Vouchers.AnyAsync(
            voucher => voucher.BookId == book.Id,
            cancellationToken);

        if (!hasVoucher)
        {
            return Conflict(new { code = "book_requires_voucher" });
        }

        book.Status = BookStatus.Published;

        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Ok(new { id = book.Id, status = book.Status });
    }

    private async Task<Book?> LockBookAsync(
        int bookId,
        CancellationToken cancellationToken)
    {
        var books = await _db.Books
            .FromSqlInterpolated(
                $"SELECT * FROM \"Books\" WHERE \"Id\" = {bookId} FOR UPDATE")
            .ToListAsync(cancellationToken);

        return books.SingleOrDefault();
    }

    private Task<bool> IsActiveAdminAsync(CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out var adminId))
        {
            return Task.FromResult(false);
        }

        return _db.Users.AnyAsync(
            user => user.Id == adminId && user.IsAdmin && !user.IsBlocked,
            cancellationToken);
    }

    private static bool IsPositiveAmount(decimal value) =>
        value > 0 &&
        value <= 9_999_999.999m &&
        decimal.Round(value, 3) == value;
}