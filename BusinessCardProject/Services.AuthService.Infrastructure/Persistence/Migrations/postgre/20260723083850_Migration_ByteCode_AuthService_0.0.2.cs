using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Services.AuthService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_AuthService_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VerifyMail",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Подтверждение аккаунта");

            migrationBuilder.CreateTable(
                name: "AccountActivationRecords",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    VerificationToken = table.Column<string>(type: "text", nullable: false, comment: "Токен верификации"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время создания записи"),
                    ExpiresAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время истечения срока"),
                    VerificationAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Время подтверждения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountActivationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountActivationRecords_UsersAuthService_UserId",
                        column: x => x.UserId,
                        principalSchema: "bytecode_auth",
                        principalTable: "UsersAuthService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivationRecords_UserId",
                schema: "bytecode_auth",
                table: "AccountActivationRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivationRecords_VerificationToken",
                schema: "bytecode_auth",
                table: "AccountActivationRecords",
                column: "VerificationToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountActivationRecords",
                schema: "bytecode_auth");

            migrationBuilder.DropColumn(
                name: "VerifyMail",
                schema: "bytecode_auth",
                table: "UsersAuthService");
        }
    }
}
