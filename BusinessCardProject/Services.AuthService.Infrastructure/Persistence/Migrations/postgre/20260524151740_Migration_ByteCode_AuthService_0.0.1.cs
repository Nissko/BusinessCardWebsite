using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Services.AuthService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_AuthService_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bytecode_auth");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "bytecode_auth",
                columns: table => new
                {
                    TokenHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    ExpiresAtUtc = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата истечения"),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.TokenHash);
                });

            migrationBuilder.CreateTable(
                name: "UsersAuthService",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Surname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Фамилия"),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Имя"),
                    NickName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Ник"),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Почта"),
                    PasswordHash = table.Column<string>(type: "text", nullable: false, comment: "Пароль"),
                    IsAuthor = table.Column<bool>(type: "boolean", nullable: false, comment: "Является ли автором"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата регистрации"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    DeletedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAuthService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "bytecode_auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Фамилия"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Фамилия")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_UsersAuthService_UserId",
                        column: x => x.UserId,
                        principalSchema: "bytecode_auth",
                        principalTable: "UsersAuthService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "bytecode_auth",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked",
                schema: "bytecode_auth",
                table: "RefreshTokens",
                columns: ["UserId", "IsRevoked"]);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "bytecode_auth",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAuthService_Email",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAuthService_NickName",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                column: "NickName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "bytecode_auth");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "bytecode_auth");

            migrationBuilder.DropTable(
                name: "UsersAuthService",
                schema: "bytecode_auth");
        }
    }
}
