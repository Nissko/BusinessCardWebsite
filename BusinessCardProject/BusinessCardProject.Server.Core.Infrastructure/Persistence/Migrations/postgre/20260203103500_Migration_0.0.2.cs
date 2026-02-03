using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardProject.Server.Core.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "dev_prod",
                table: "Themes",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                comment: "Порядок отображения");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dev_prod",
                table: "Themes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Нужно ли отображать");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                schema: "dev_prod",
                table: "Themes",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Платная ли тема");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "IsFree",
                schema: "dev_prod",
                table: "Themes");
        }
    }
}
