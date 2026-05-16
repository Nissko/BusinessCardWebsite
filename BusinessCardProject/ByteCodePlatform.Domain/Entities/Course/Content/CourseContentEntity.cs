using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Module;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities.Course.Content
{
    public class CourseContentEntity : Entity
    {
        public CourseContentEntity(string name, string linkOnRutube, string linkOnVk, string linkOnYouTube,
            string imgUrl, Instant createdAt, Guid courseModuleId, Instant? updatedAt = null)
        {
            Name = name;
            LinkOnRutube = linkOnRutube;
            LinkOnVk = linkOnVk;
            LinkOnYouTube = linkOnYouTube;
            ImgUrl = imgUrl;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CourseModuleId = courseModuleId;
            ContentFieldProperties = new HashSet<CourseContentFieldPropertyEntity>();
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Ссылка на рутуб
        /// </summary>
        public string LinkOnRutube { get; private set; }

        /// <summary>
        /// Ссылка на вк
        /// </summary>
        public string LinkOnVk { get; private set; }

        /// <summary>
        /// Ссылка на ютуб
        /// </summary>
        public string LinkOnYouTube { get; private set; }

        /// <summary>
        /// Ссылка на обложку
        /// </summary>
        public string ImgUrl { get; private set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public Instant CreatedAt { get; private set; }

        /// <summary>
        /// Дата редактирования
        /// </summary>
        public Instant? UpdatedAt { get; private set; }

        /// <summary>
        /// Модуль к которому относится курс
        /// </summary>
        public virtual CourseModuleEntity CourseModule { get; private set; }

        public Guid CourseModuleId { get; private set; }

        public virtual ICollection<CourseContentFieldPropertyEntity> ContentFieldProperties { get; private set; }

        public void Update(string? name, string? linkOnRutube, string? linkOnVk, string? linkOnYouTube,
            string? imgUrl, Guid? courseModuleId)
        {
            Name = string.IsNullOrEmpty(name) ? Name : name;
            LinkOnRutube = string.IsNullOrEmpty(linkOnRutube) ? LinkOnRutube : linkOnRutube;
            LinkOnVk = string.IsNullOrEmpty(linkOnVk) ? LinkOnVk : linkOnVk;
            LinkOnYouTube = string.IsNullOrEmpty(linkOnYouTube) ? LinkOnYouTube : linkOnYouTube;
            ImgUrl = string.IsNullOrEmpty(imgUrl) ? ImgUrl : imgUrl;
            CourseModuleId = courseModuleId ?? CourseModuleId;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }
    }
}