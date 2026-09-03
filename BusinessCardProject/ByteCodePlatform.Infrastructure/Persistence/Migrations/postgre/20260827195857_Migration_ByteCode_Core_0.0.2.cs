using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteCodePlatform.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_Core_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutUs",
                schema: "bytecode_core",
                table: "Authors",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                comment: "Об авторе");

            migrationBuilder.AddColumn<string>(
                name: "AvatarId",
                schema: "bytecode_core",
                table: "Authors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Аватар");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "bytecode_core",
                table: "Authors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Имя");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "bytecode_core",
                table: "Authors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Фамилия");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutUs",
                schema: "bytecode_core",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                schema: "bytecode_core",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "bytecode_core",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "bytecode_core",
                table: "Authors");
        }
    }
}
