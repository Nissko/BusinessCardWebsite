using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.MailService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_MailService_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bytecode_email_notifications");

            migrationBuilder.CreateTable(
                name: "EmailContentTemplates",
                schema: "bytecode_email_notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона"),
                    Subject = table.Column<string>(type: "text", nullable: false, comment: "Название темы из шаблона"),
                    Body = table.Column<string>(type: "text", nullable: false, comment: "Тело письма")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailContentTemplates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailContentTemplates",
                schema: "bytecode_email_notifications");
        }
    }
}
