using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouponBook.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeBranchFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderUsers_Branches_BranchId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderUsers_BranchId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderUsers_ProviderId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderUsers_UserId_ProviderId_BranchId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_Branches_ProviderId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProviderUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "ProviderUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Branches_ProviderId_Id",
                table: "Branches",
                columns: new[] { "ProviderId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderUsers_ProviderId_BranchId",
                table: "ProviderUsers",
                columns: new[] { "ProviderId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderUsers_UserId",
                table: "ProviderUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderUsers_Branches_ProviderId_BranchId",
                table: "ProviderUsers",
                columns: new[] { "ProviderId", "BranchId" },
                principalTable: "Branches",
                principalColumns: new[] { "ProviderId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderUsers_Branches_ProviderId_BranchId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderUsers_ProviderId_BranchId",
                table: "ProviderUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderUsers_UserId",
                table: "ProviderUsers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Branches_ProviderId_Id",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "ProviderUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProviderUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderUsers_BranchId",
                table: "ProviderUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderUsers_ProviderId",
                table: "ProviderUsers",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderUsers_UserId_ProviderId_BranchId",
                table: "ProviderUsers",
                columns: new[] { "UserId", "ProviderId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ProviderId",
                table: "Branches",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderUsers_Branches_BranchId",
                table: "ProviderUsers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
