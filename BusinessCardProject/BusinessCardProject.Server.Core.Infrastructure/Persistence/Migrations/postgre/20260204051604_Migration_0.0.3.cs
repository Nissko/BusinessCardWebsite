using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardProject.Server.Core.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThemeRecommendations",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseThemeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Рекомендация")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemeRecommendations_Themes_CourseThemeId",
                        column: x => x.CourseThemeId,
                        principalSchema: "dev_prod",
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThemeRecommendations_CourseThemeId",
                schema: "dev_prod",
                table: "ThemeRecommendations",
                column: "CourseThemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeRecommendations",
                schema: "dev_prod");
        }
    }
}
