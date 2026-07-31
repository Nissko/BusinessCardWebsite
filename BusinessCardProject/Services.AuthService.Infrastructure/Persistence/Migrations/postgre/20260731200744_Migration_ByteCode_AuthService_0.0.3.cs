using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Services.AuthService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_AuthService_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountActivationRecords_UserId",
                schema: "bytecode_auth",
                table: "AccountActivationRecords");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                comment: "Хеш пароля",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Пароль");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "bytecode_auth",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "Идентификатор пользователя",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Фамилия");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "bytecode_auth",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "Идентификатор роли",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Фамилия");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор пользователя"),
                    Action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Действие"),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Подробности"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время записи"),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false, comment: "IP адрес"),
                    UserAgent = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false, comment: "User-Agent")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedLoginAttempts",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор пользователя"),
                    AttemptedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время попытки входа")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор пользователя"),
                    ResetToken = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "Токен сброса пароля"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время создания токена"),
                    ExpiresAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Время истечения токена"),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Использован ли токен")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiresAtUtc",
                schema: "bytecode_auth",
                table: "RefreshTokens",
                column: "ExpiresAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivationRecords_ExpiresAt",
                schema: "bytecode_auth",
                table: "AccountActivationRecords",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivationRecords_UserId_ExpiresAt",
                schema: "bytecode_auth",
                table: "AccountActivationRecords",
                columns: new[] { "UserId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                schema: "bytecode_auth",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId_CreatedAt",
                schema: "bytecode_auth",
                table: "AuditLogs",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FailedLoginAttempts_AttemptedAt",
                schema: "bytecode_auth",
                table: "FailedLoginAttempts",
                column: "AttemptedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FailedLoginAttempts_UserId_AttemptedAt",
                schema: "bytecode_auth",
                table: "FailedLoginAttempts",
                columns: new[] { "UserId", "AttemptedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_ExpiresAt",
                schema: "bytecode_auth",
                table: "PasswordResets",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UserId_IsUsed",
                schema: "bytecode_auth",
                table: "PasswordResets",
                columns: new[] { "UserId", "IsUsed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "bytecode_auth");

            migrationBuilder.DropTable(
                name: "FailedLoginAttempts",
                schema: "bytecode_auth");

            migrationBuilder.DropTable(
                name: "PasswordResets",
                schema: "bytecode_auth");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ExpiresAtUtc",
                schema: "bytecode_auth",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountActivationRecords_ExpiresAt",
                schema: "bytecode_auth",
                table: "AccountActivationRecords");

            migrationBuilder.DropIndex(
                name: "IX_AccountActivationRecords_UserId_ExpiresAt",
                schema: "bytecode_auth",
                table: "AccountActivationRecords");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                type: "text",
                nullable: false,
                comment: "Пароль",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldComment: "Хеш пароля");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "bytecode_auth",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "Фамилия",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Идентификатор пользователя");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "bytecode_auth",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "Фамилия",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Идентификатор роли");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivationRecords_UserId",
                schema: "bytecode_auth",
                table: "AccountActivationRecords",
                column: "UserId");
        }
    }
}
