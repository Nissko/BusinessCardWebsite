using System;
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
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeletion",
                schema: "dev_prod",
                table: "UserProfiles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Дата деактивации аккаунта");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfRegistered",
                schema: "dev_prod",
                table: "UserProfiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Дата регистрации");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeletion",
                schema: "dev_prod",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DateOfRegistered",
                schema: "dev_prod",
                table: "UserProfiles");
        }
    }
}
