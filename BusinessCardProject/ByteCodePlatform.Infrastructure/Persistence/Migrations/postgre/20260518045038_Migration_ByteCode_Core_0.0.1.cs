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
                name: "FieldPropertyTypes",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название свойства"),
                    Description = table.Column<string>(type: "text", nullable: false, comment: "Описание свойства")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldPropertyTypes", x => x.Id);
                });

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
                name: "UsersService",
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
                    table.PrimaryKey("PK_UsersService", x => x.Id);
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
                        name: "FK_Authors_UsersService_UserId",
                        column: x => x.UserId,
                        principalSchema: "bytecode_core",
                        principalTable: "UsersService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Price = table.Column<double>(type: "double precision", nullable: false, comment: "Текущая цена"),
                    OldPrice = table.Column<double>(type: "double precision", nullable: true, comment: "Старая цена"),
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
                name: "CourseThemeFieldProperties",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false, comment: "Значение"),
                    FieldPropertyTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseThemeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseThemeFieldProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseThemeFieldProperties_CourseThemes_CourseThemeId",
                        column: x => x.CourseThemeId,
                        principalSchema: "bytecode_core",
                        principalTable: "CourseThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseThemeFieldProperties_FieldPropertyTypes_FieldProperty~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypes",
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

            migrationBuilder.CreateTable(
                name: "CourseModuleFieldProperties",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false, comment: "Значение"),
                    FieldPropertyTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseModuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModuleFieldProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModuleFieldProperties_CourseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalSchema: "bytecode_core",
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseModuleFieldProperties_FieldPropertyTypes_FieldPropert~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseContentFieldProperties",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false, comment: "Значение"),
                    FieldPropertyTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContentFieldProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseContentFieldProperties_CourseContents_CourseContentId",
                        column: x => x.CourseContentId,
                        principalSchema: "bytecode_core",
                        principalTable: "CourseContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseContentFieldProperties_FieldPropertyTypes_FieldProper~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypes",
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
                name: "IX_CourseContentFieldProperties_CourseContentId",
                schema: "bytecode_core",
                table: "CourseContentFieldProperties",
                column: "CourseContentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseContentFieldProperties_FieldPropertyTypeId",
                schema: "bytecode_core",
                table: "CourseContentFieldProperties",
                column: "FieldPropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_CourseModuleId",
                schema: "bytecode_core",
                table: "CourseContents",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModuleFieldProperties_CourseModuleId",
                schema: "bytecode_core",
                table: "CourseModuleFieldProperties",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModuleFieldProperties_FieldPropertyTypeId",
                schema: "bytecode_core",
                table: "CourseModuleFieldProperties",
                column: "FieldPropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseThemeId",
                schema: "bytecode_core",
                table: "CourseModules",
                column: "CourseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThemeFieldProperties_CourseThemeId",
                schema: "bytecode_core",
                table: "CourseThemeFieldProperties",
                column: "CourseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThemeFieldProperties_FieldPropertyTypeId",
                schema: "bytecode_core",
                table: "CourseThemeFieldProperties",
                column: "FieldPropertyTypeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UsersService_Email",
                schema: "bytecode_core",
                table: "UsersService",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersService_NickName",
                schema: "bytecode_core",
                table: "UsersService",
                column: "NickName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseContentFieldProperties",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseModuleFieldProperties",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseThemeFieldProperties",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseContents",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "FieldPropertyTypes",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseModules",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "CourseThemes",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguageCategories",
                schema: "bytecode_core");

            migrationBuilder.DropTable(
                name: "UsersService",
                schema: "bytecode_core");
        }
    }
}
