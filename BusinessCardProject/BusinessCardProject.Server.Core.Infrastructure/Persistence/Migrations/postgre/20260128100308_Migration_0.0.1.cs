using BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting;
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
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название типа курса")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeUserRoles",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название роли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeUserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Settings = table.Column<UserSetting>(type: "jsonb", nullable: false, comment: "Настройки пользователя"),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак работоспособности профиля"),
                    isBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак блокировки"),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Пароль"),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Фамилия"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Имя"),
                    Patronymic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Отчество"),
                    NickName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Почта"),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Почта")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgrammingLanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, comment: "Описание"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название"),
                    TypeOfCourse = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_ProgrammingLanguages_ProgrammingLanguageId",
                        column: x => x.ProgrammingLanguageId,
                        principalSchema: "dev_prod",
                        principalTable: "ProgrammingLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Ид роли пользователя")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Roles_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalSchema: "dev_prod",
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseThemeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, comment: "Описание"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Themes_CourseThemeId",
                        column: x => x.CourseThemeId,
                        principalSchema: "dev_prod",
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoCourse",
                schema: "dev_prod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseAuthorId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseAuthorId = table.Column<Guid>(type: "uuid", nullable: false, comment: "ID автора"),
                    CourseDatePublished = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата публикации"),
                    CourseDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Описание"),
                    CourseDiscount = table.Column<int>(type: "integer", nullable: false, comment: "Размер скидки"),
                    CourseImg = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Изображение"),
                    CourseName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Название"),
                    CoursePrice = table.Column<double>(type: "double precision", nullable: false, comment: "Стоимость"),
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
                    table.ForeignKey(
                        name: "FK_VideoCourse_CourseAuthors_CourseAuthorId1",
                        column: x => x.CourseAuthorId1,
                        principalSchema: "dev_prod",
                        principalTable: "CourseAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoCourse_Modules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalSchema: "dev_prod",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAuthors_Nickname",
                schema: "dev_prod",
                table: "CourseAuthors",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseThemeId",
                schema: "dev_prod",
                table: "Modules",
                column: "CourseThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_ProgrammingLanguageId",
                schema: "dev_prod",
                table: "Themes",
                column: "ProgrammingLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfCourses_Id",
                schema: "dev_prod",
                table: "TypeOfCourses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TypeUserRoles_Id",
                schema: "dev_prod",
                table: "TypeUserRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_UserProfileId",
                schema: "dev_prod",
                table: "User_Roles",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Email",
                schema: "dev_prod",
                table: "UserProfiles",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_NickName",
                schema: "dev_prod",
                table: "UserProfiles",
                column: "NickName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoCourse_CourseAuthorId1",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "CourseAuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCourse_CourseModuleId",
                schema: "dev_prod",
                table: "VideoCourse",
                column: "CourseModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeOfCourses",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "TypeUserRoles",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "User_Roles",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "VideoCourse",
                schema: "dev_prod");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "dev_prod");

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
        }
    }
}
