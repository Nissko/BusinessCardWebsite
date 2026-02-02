using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course;

/// <summary>
/// Конфигурация для таблицы с курсами
/// </summary>
internal class VideoCourseConfiguration : IEntityTypeConfiguration<VideoCourseEntity>
{
    public void Configure(EntityTypeBuilder<VideoCourseEntity> builder)
    {
        builder.ToTable("VideoCourse");
        builder.HasKey(x => x.Id);

        builder.Property<string>("_linkCourseOnYoutube")
            .HasColumnName("LinkOnYoutube")
            .IsRequired()
            .HasComment("Ссылка на ютуб");

        builder.Property<string>("_linkCourseOnRutube")
            .HasColumnName("LinkOnRutube")
            .IsRequired()
            .HasComment("Ссылка на рутуб");

        builder.Property<string>("_linkCourseOnVkVideo")
            .HasColumnName("LinkOnVkVideo")
            .IsRequired()
            .HasComment("Ссылка на ВК видео");

        builder.Property<string>("_courseName")
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Название");

        builder.Property<string>("_courseDescription")
            .HasColumnName("Description")
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Описание");

        builder.Property<string>("_courseImg")
            .HasColumnName("Img")
            .HasMaxLength(200)
            .HasComment("Изображение");

        builder.Property("_courseDatePublished")
            .HasColumnName("DatePublished")
            .IsRequired()
            .HasComment("Дата публикации");

        builder.Property<double>("_coursePrice")
            .HasColumnName("Price")
            .IsRequired()
            .HasComment("Стоимость");

        builder.Property<int>("_courseDiscount")
            .HasColumnName("Discount")
            .IsRequired()
            .HasComment("Размер скидки");

        builder.Property<double>("_courseRate")
            .HasColumnName("Rate")
            .IsRequired()
            .HasComment("Рейтинг");

        builder.Property<bool>("_isFree")
            .HasColumnName("IsFree")
            .IsRequired()
            .HasComment("Признак, платный курс или нет");

        builder.Property<bool>("IsShow")
            .HasColumnName("IsShow")
            .HasDefaultValue(false)
            .HasComment("Признак, будет ли показываться курс на странице");

        builder.Property("DisplayOrder")
            .HasColumnName("DisplayOrder")
            .HasDefaultValue(0)
            .HasComment("Порядок сортировки");

        builder.HasOne(x => x.CourseModule)
            .WithMany(x => x.VideoCourses)
            .HasForeignKey(x=>x.CourseModuleId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.CourseAuthor)
            .WithMany(x => x.VideoCourses)
            .HasForeignKey(x=>x.CourseAuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        #region Индексы

        builder.HasIndex(x=>x.CourseModuleId);
        builder.HasIndex(x=>x.CourseAuthorId); 

        #endregion
    }
}