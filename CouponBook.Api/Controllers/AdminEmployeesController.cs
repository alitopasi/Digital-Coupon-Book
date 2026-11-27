using CouponBook.Data;
using CouponBook.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CouponBook.Api.Controllers;

[ApiController]
[Route("api/admin/employees")]
[Authorize(Roles = "Admin")]
public class AdminEmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminEmployeesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPut("{userId:int}/branches/{branchId:int}")]
    public async Task<IActionResult> AssignEmployee(
        int userId,
        int branchId,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out var adminId))
        {
            return Forbid();
        }

        var isActiveAdmin = await _db.Users.AnyAsync(
            user => user.Id == adminId && user.IsAdmin && !user.IsBlocked,
            cancellationToken);

        if (!isActiveAdmin)
        {
            return Forbid();
        }

        var branch = await _db.Branches
            .AsNoTracking()
            .Include(x => x.Provider)
            .SingleOrDefaultAsync(x => x.Id == branchId, cancellationToken);

        if (branch is null)
        {
            return NotFound(new { code = "branch_not_found" });
        }

        if (branch.Status != AccountStatus.Active ||
            branch.Provider.Status != AccountStatus.Active)
        {
            return Conflict(new { code = "branch_or_provider_inactive" });
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        // Serialize assignment requests for the same account.
        var users = await _db.Users
            .FromSqlInterpolated(
                $"SELECT * FROM \"Users\" WHERE \"Id\" = {userId} FOR UPDATE")
            .ToListAsync(cancellationToken);

        var user = users.SingleOrDefault();

        if (user is null)
        {
            return NotFound(new { code = "user_not_found" });
        }

        if (user.IsBlocked)
        {
            return Conflict(new { code = "user_blocked" });
        }

        var assignment = await _db.ProviderUsers.SingleOrDefaultAsync(
            x => x.UserId == userId, cancellationToken);

        if (assignment is not null && assignment.BranchId != branch.Id)
        {
            return Conflict(new { code = "employee_already_assigned" });
        }

        if (assignment is null)
        {
            assignment = new ProviderUser
            {
                UserId = user.Id,
                ProviderId = branch.ProviderId,
                BranchId = branch.Id
            };

            _db.ProviderUsers.Add(assignment);
        }

        assignment.Status = AccountStatus.Active;
        user.IsCustomer = true;

        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Ok(new
        {
            userId = user.Id,
            providerId = assignment.ProviderId,
            branchId = assignment.BranchId,
            isCustomer = user.IsCustomer,
            isProvider = true
        });
    }
}