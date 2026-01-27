using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasColumnName("LinkCourseOnYoutube")
            .IsRequired()
            .HasComment("Ссылка на ютуб");

        builder.Property<string>("_linkCourseOnRutube")
            .HasColumnName("LinkCourseOnRutube")
            .IsRequired()
            .HasComment("Ссылка на рутуб");

        builder.Property<string>("_linkCourseOnVkVideo")
            .HasColumnName("LinkCourseOnVkVideo")
            .IsRequired()
            .HasComment("Ссылка на ВК видео");

        builder.Property<Guid>("_courseAuthorId")
            .HasColumnName("CourseAuthorId")
            .IsRequired()
            .HasComment("ID автора");

        builder.Property<string>("_courseName")
            .HasColumnName("CourseName")
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Название");

        builder.Property<string>("_courseDescription")
            .HasColumnName("CourseDescription")
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Описание");

        builder.Property<string>("_courseImg")
            .HasColumnName("CourseImg")
            .HasMaxLength(200)
            .HasComment("Изображение");

        builder.Property<DateTime>("_courseDatePublished")
            .HasColumnName("CourseDatePublished")
            .IsRequired()
            .HasComment("Дата публикации");

        builder.Property<double>("_coursePrice")
            .HasColumnName("CoursePrice")
            .IsRequired()
            .HasComment("Стоимость");

        builder.Property<int>("_courseDiscount")
            .HasColumnName("CourseDiscount")
            .IsRequired()
            .HasComment("Размер скидки");

        builder.Property<double>("_courseRate")
            .HasColumnName("CourseRate")
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
            .HasForeignKey("_courseModuleId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.CourseAuthor)
            .WithMany(x => x.VideoCourses)
            .HasForeignKey("_courseAuthorId")
            .OnDelete(DeleteBehavior.Cascade);

        #region Индексы

        builder.HasIndex("_courseModuleId");
        builder.HasIndex("_courseAuthorId"); 

        #endregion
    }
}