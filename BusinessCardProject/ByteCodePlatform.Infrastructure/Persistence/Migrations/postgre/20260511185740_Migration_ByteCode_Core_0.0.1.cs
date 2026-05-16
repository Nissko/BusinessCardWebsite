using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace ByteCodePlatform.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_Core_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bytecode_core");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Surname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Фамилия"),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Имя"),
                    NickName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Ник"),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Почта"),
                    IsAuthor = table.Column<bool>(type: "boolean", nullable: false, comment: "Является ли автором"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата регистрации"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    DeletedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата регистрации"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    DeletedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "bytecode_core",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                schema: "bytecode_core",
                table: "Authors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "bytecode_core",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NickName",
                schema: "bytecode_core",
                table: "Users",
                column: "NickName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "bytecode_core");
        }
    }
}
