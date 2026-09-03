using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.AuthService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_AuthService_005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                schema: "bytecode_auth",
                table: "RefreshTokens",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_UserAgent",
                schema: "bytecode_auth",
                table: "RefreshTokens",
                columns: new[] { "UserId", "UserAgent" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId_UserAgent",
                schema: "bytecode_auth",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                schema: "bytecode_auth",
                table: "RefreshTokens");
        }
    }
}
