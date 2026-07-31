using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.MailService.Domain.Entities;

namespace Services.MailService.Infrastructure.Configurations
{
    public class EmailContentTemplateConfiguration : IEntityTypeConfiguration<EmailContentTemplateEntity>
    {
        public void Configure(EntityTypeBuilder<EmailContentTemplateEntity> builder)
        {
            builder.ToTable("EmailContentTemplates");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TemplateName)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Название шаблона");

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(256)
                .HasComment("Название темы из шаблона");

            builder.Property(x => x.Body)
                .IsRequired()
                .IsUnicode(false)
                .HasComment("Тело письма");
        }
    }
}