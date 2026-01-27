using System;
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
            migrationBuilder.DropForeignKey(
                name: "FK_Themes_TypeOfCourses__typeOfCourseId",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Themes__typeOfCourseId",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "_typeOfCourseId",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.AddColumn<Guid>(
                name: "TypeOfCourse",
                schema: "dev_prod",
                table: "Themes",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfCourse",
                schema: "dev_prod",
                table: "Themes");

            migrationBuilder.AddColumn<Guid>(
                name: "_typeOfCourseId",
                schema: "dev_prod",
                table: "Themes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Themes__typeOfCourseId",
                schema: "dev_prod",
                table: "Themes",
                column: "_typeOfCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_TypeOfCourses__typeOfCourseId",
                schema: "dev_prod",
                table: "Themes",
                column: "_typeOfCourseId",
                principalSchema: "dev_prod",
                principalTable: "TypeOfCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
