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
            migrationBuilder.AlterColumn<double>(
                name: "CoursePrice",
                schema: "dev_prod",
                table: "VideoCourse",
                type: "double precision",
                nullable: false,
                comment: "Стоимость",
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<Guid>(
                name: "_courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CourseAuthors",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguages",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountSelectedUser = table.Column<int>(type: "integer", nullable: false, comment: "Количество людей использующий ЯП"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название языка программирования")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfCourses",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    _typeOfCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    _programmingLanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, comment: "Описание"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_ProgrammingLanguages__programmingLanguageId",
                        column: x => x._programmingLanguageId,
                        principalSchema: "dev_prod",
                        principalTable: "ProgrammingLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Themes_TypeOfCourses__typeOfCourseId",
                        column: x => x._typeOfCourseId,
                        principalSchema: "dev_prod",
                        principalTable: "TypeOfCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    _courseThemeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, comment: "Описание"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Themes__courseThemeId",
                        column: x => x._courseThemeId,
                        principalSchema: "dev_prod",
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoCourse__courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "_courseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCourse_CourseAuthorId",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "CourseAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAuthors_Nickname",
                schema: "dev_prod",
                table: "CourseAuthors",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules__courseThemeId",
                schema: "dev_prod",
                table: "Modules",
                column: "_courseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes__programmingLanguageId",
                schema: "dev_prod",
                table: "Themes",
                column: "_programmingLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes__typeOfCourseId",
                schema: "dev_prod",
                table: "Themes",
                column: "_typeOfCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfCourses_Id",
                schema: "dev_prod",
                table: "TypeOfCourses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCourse_CourseAuthors_CourseAuthorId",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "CourseAuthorId",
                principalSchema: "dev_prod",
                principalTable: "CourseAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCourse_Modules__courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "_courseModuleId",
                principalSchema: "dev_prod",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoCourse_CourseAuthors_CourseAuthorId",
                schema: "dev_prod",
                table: "VideoCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCourse_Modules__courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse");

            migrationBuilder.DropTable(
                name: "CourseAuthors",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "Themes",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguages",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "TypeOfCourses",
                schema: "dev_prod");

            migrationBuilder.DropIndex(
                name: "IX_VideoCourse__courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse");

            migrationBuilder.DropIndex(
                name: "IX_VideoCourse_CourseAuthorId",
                schema: "dev_prod",
                table: "VideoCourse");

            migrationBuilder.DropColumn(
                name: "_courseModuleId",
                schema: "dev_prod",
                table: "VideoCourse");

            migrationBuilder.AlterColumn<double>(
                name: "CoursePrice",
                schema: "dev_prod",
                table: "VideoCourse",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComment: "Стоимость");
        }
    }
}
