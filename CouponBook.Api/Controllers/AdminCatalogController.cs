using System.ComponentModel.DataAnnotations;
using CouponBook.Data;
using CouponBook.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CouponBook.Api.Controllers;

public record CreateCityRequest(
    [Required, StringLength(100)] string NameAr,
    [Required, StringLength(100)] string NameEn);

public record CreateCategoryRequest(
    [Required, StringLength(100)] string NameAr,
    [Required, StringLength(100)] string NameEn);

public record CreateProviderRequest(
    [Required, StringLength(150)] string Name,
    [Range(1, int.MaxValue)] int CategoryId);

public record CreateBranchRequest(
    [Required, StringLength(150)] string Name,
    [Required, StringLength(250)] string Address);

[ApiController]
[Route("api/admin/catalog")]
[Authorize(Roles = "Admin")]
public class AdminCatalogController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminCatalogController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("cities")]
    public async Task<IActionResult> CreateCity(
        CreateCityRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        var city = new City
        {
            NameAr = request.NameAr.Trim(),
            NameEn = request.NameEn.Trim(),
            IsActive = true
        };

        _db.Cities.Add(city);
        await _db.SaveChangesAsync(cancellationToken);

        return StatusCode(201, new { id = city.Id });
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        var category = new Category
        {
            NameAr = request.NameAr.Trim(),
            NameEn = request.NameEn.Trim(),
            IsActive = true
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync(cancellationToken);

        return StatusCode(201, new { id = category.Id });
    }

    [HttpPost("providers")]
    public async Task<IActionResult> CreateProvider(
        CreateProviderRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        var categoryExists = await _db.Categories.AnyAsync(
            x => x.Id == request.CategoryId && x.IsActive,
            cancellationToken);

        if (!categoryExists)
        {
            return NotFound(new { code = "active_category_not_found" });
        }

        var provider = new Provider
        {
            Name = request.Name.Trim(),
            CategoryId = request.CategoryId,
            Status = AccountStatus.Active
        };

        _db.Providers.Add(provider);
        await _db.SaveChangesAsync(cancellationToken);

        return StatusCode(201, new { id = provider.Id });
    }

    [HttpPost("providers/{providerId:int}/branches")]
    public async Task<IActionResult> CreateBranch(
        int providerId,
        CreateBranchRequest request,
        CancellationToken cancellationToken)
    {
        if (!await IsActiveAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        var providerExists = await _db.Providers.AnyAsync(
            x => x.Id == providerId && x.Status == AccountStatus.Active,
            cancellationToken);

        if (!providerExists)
        {
            return NotFound(new { code = "active_provider_not_found" });
        }

        var branch = new Branch
        {
            ProviderId = providerId,
            Name = request.Name.Trim(),
            Address = request.Address.Trim(),
            Status = AccountStatus.Active
        };

        _db.Branches.Add(branch);
        await _db.SaveChangesAsync(cancellationToken);

        return StatusCode(201, new { id = branch.Id });
    }

    private Task<bool> IsActiveAdminAsync(CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out var adminId))
        {
            return Task.FromResult(false);
        }

        return _db.Users.AnyAsync(
            x => x.Id == adminId && x.IsAdmin && !x.IsBlocked,
            cancellationToken);
    }
}