using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.MailService.Infrastructure.Persistence.Migrations.postgre
{
    /// <inheritdoc />
    public partial class Migration_ByteCode_MailService_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                schema: "bytecode_email_notifications",
                table: "EmailContentTemplates",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Название шаблона",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название шаблона");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "bytecode_email_notifications",
                table: "EmailContentTemplates",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                comment: "Название темы из шаблона",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название темы из шаблона");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                schema: "bytecode_email_notifications",
                table: "EmailContentTemplates",
                type: "text",
                nullable: false,
                comment: "Название шаблона",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Название шаблона");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "bytecode_email_notifications",
                table: "EmailContentTemplates",
                type: "text",
                nullable: false,
                comment: "Название темы из шаблона",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldComment: "Название темы из шаблона");
        }
    }
}
