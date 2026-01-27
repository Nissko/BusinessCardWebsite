using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardProject.Server.Core.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dev_prod");

            migrationBuilder.CreateTable(
                name: "VideoCourse",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseAuthorId = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID автора"),
                    CourseDatePublished = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата публикации"),
                    CourseDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Описание"),
                    CourseDiscount = table.Column<int>(type: "integer", nullable: false, comment: "Размер скидки"),
                    CourseImg = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Изображение"),
                    CourseName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название"),
                    CoursePrice = table.Column<double>(type: "double precision", nullable: false),
                    CourseRate = table.Column<double>(type: "double precision", nullable: false, comment: "Рейтинг"),
                    IsFree = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак, платный курс или нет"),
                    LinkCourseOnRutube = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на рутуб"),
                    LinkCourseOnVkVideo = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на ВК видео"),
                    LinkCourseOnYoutube = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на ютуб"),
                    IsShow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак, будет ли показываться курс на странице"),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Порядок сортировки")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoCourse", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoCourse",
                schema: "dev_prod");
        }
    }
}
