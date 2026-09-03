using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.AuthService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_AuthService_006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarId",
                schema: "bytecode_auth",
                table: "UsersAuthService",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Идентификатор аватара");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarId",
                schema: "bytecode_auth",
                table: "UsersAuthService");
        }
    }
}
