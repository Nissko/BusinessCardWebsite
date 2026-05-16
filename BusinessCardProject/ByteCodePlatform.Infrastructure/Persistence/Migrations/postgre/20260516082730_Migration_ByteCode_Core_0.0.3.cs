using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteCodePlatform.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_Core_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldPropertyTypeEntity",
                schema: "bytecode_core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldPropertyTypeEntity", x => x.Id);
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
                        name: "FK_CourseContentFieldProperties_FieldPropertyTypeEntity_FieldP~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypeEntity",
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
                        name: "FK_CourseModuleFieldProperties_FieldPropertyTypeEntity_FieldPr~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypeEntity",
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
                        name: "FK_CourseThemeFieldProperties_FieldPropertyTypeEntity_FieldPro~",
                        column: x => x.FieldPropertyTypeId,
                        principalSchema: "bytecode_core",
                        principalTable: "FieldPropertyTypeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_CourseThemeFieldProperties_CourseThemeId",
                schema: "bytecode_core",
                table: "CourseThemeFieldProperties",
                column: "CourseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThemeFieldProperties_FieldPropertyTypeId",
                schema: "bytecode_core",
                table: "CourseThemeFieldProperties",
                column: "FieldPropertyTypeId");
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
                name: "FieldPropertyTypeEntity",
                schema: "bytecode_core");
        }
    }
}
