using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace ByteCodePlatform.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_Core_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgrammingLanguageCategories",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false, comment: "Название модуля")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguageCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseThemes",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    AvatarUrl = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на обложку"),
                    Price = table.Column<decimal>(type: "numeric", nullable: false, comment: "Текущая цена"),
                    OldPrice = table.Column<decimal>(type: "numeric", nullable: true, comment: "Старая цена"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата добавления"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgrammingLanguageCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseThemes_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "bytecode_core",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseThemes_ProgrammingLanguageCategories_ProgrammingLangu~",
                        column: x => x.ProgrammingLanguageCategoryId,
                        principalSchema: "bytecode_core",
                        principalTable: "ProgrammingLanguageCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseModules",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название модуля"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата добавления"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления"),
                    CourseThemeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_CourseThemes_CourseThemeId",
                        column: x => x.CourseThemeId,
                        principalSchema: "bytecode_core",
                        principalTable: "CourseThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseContents",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название видео"),
                    LinkOnRutube = table.Column<string>(type: "text", nullable: false, comment: "Ссылка рутуб"),
                    LinkOnVk = table.Column<string>(type: "text", nullable: false, comment: "Ссылка вк видео"),
                    LinkOnYouTube = table.Column<string>(type: "text", nullable: false, comment: "Ссылка ютуб"),
                    ImgUrl = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на обложку видео"),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата добавления"),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    CourseModuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseContents_CourseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalSchema: "bytecode_core",
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_CourseModuleId",
                schema: "bytecode_core",
                table: "CourseContents",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseThemeId",
                schema: "bytecode_core",
                table: "CourseModules",
                column: "CourseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThemes_AuthorId",
                schema: "bytecode_core",
                table: "CourseThemes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThemes_ProgrammingLanguageCategoryId",
                schema: "bytecode_core",
                table: "CourseThemes",
                column: "ProgrammingLanguageCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseContents",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseModules",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseThemes",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguageCategories",
                schema: "bytecode_core");
        }
    }
}
