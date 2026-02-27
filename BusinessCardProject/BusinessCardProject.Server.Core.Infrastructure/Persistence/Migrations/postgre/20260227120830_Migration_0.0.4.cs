using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardProject.Server.Core.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "dev_prod",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                comment: "Порядок отображения");

            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                schema: "dev_prod",
                table: "Modules",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Вывод");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "dev_prod",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsShow",
                schema: "dev_prod",
                table: "Modules");
        }
    }
}
