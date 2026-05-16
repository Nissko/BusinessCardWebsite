using ByteCodePlatform.Domain.Entities.Course.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration
{
    public class CourseContentConfiguration : IEntityTypeConfiguration<CourseContentEntity>
    {
        public void Configure(EntityTypeBuilder<CourseContentEntity> builder)
        {
            builder.ToTable("CourseContents");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasComment("Название видео");

            builder.Property(x => x.LinkOnRutube)
                .HasComment("Ссылка рутуб");

            builder.Property(x => x.LinkOnVk)
                .HasComment("Ссылка вк видео");

            builder.Property(x => x.LinkOnYouTube)
                .HasComment("Ссылка ютуб");

            builder.Property(x => x.ImgUrl)
                .HasComment("Ссылка на обложку видео");

            builder.Property(x => x.CreatedAt)
                .HasComment("Дата добавления");

            builder.Property(x => x.UpdatedAt)
                .HasComment("Дата изменения");

            builder.HasOne(x => x.CourseModule)
                .WithMany(x => x.Contents)
                .HasForeignKey(x => x.CourseModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}