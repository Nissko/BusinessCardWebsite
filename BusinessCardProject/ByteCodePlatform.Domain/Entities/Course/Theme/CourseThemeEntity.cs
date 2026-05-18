using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Enums;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities.Course.Theme
{
    public class CourseThemeEntity : Entity
    {
        public CourseThemeEntity(string name, string description, string avatarUrl, double price, double? oldPrice,
            Guid programmingLanguageCategoryId, Guid authorId, Instant createdAt, Instant? updatedAt = null)
        {
            Name = name;
            Description = description;
            AvatarUrl = avatarUrl;
            Price = price;
            OldPrice = oldPrice;
            ProgrammingLanguageCategoryId = programmingLanguageCategoryId;
            AuthorId = authorId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CourseModules = new HashSet<CourseModuleEntity>();
            ThemeFieldProperties = new HashSet<CourseThemeFieldPropertyEntity>();
        }

        /// <summary>
        /// Название темы
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Обложка
        /// </summary>
        public string AvatarUrl { get; private set; }

        /// <summary>
        /// Цена
        /// </summary>
        public double Price { get; private set; }

        /// <summary>
        /// Старая цена из которой идет расчет скидки
        /// </summary>
        public double? OldPrice { get; private set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public Instant CreatedAt { get; private set; }

        /// <summary>
        /// Дата редактирования
        /// </summary>
        public Instant? UpdatedAt { get; private set; }
    
        /// <summary>
        /// Автор
        /// </summary>
        public virtual AuthorEntity Author { get; private set; }
        public Guid AuthorId { get; private set; }
        
        /// <summary>
        /// Язык программирования темы курса
        /// </summary>
        public virtual ProgrammingLanguageCategoryEntity ProgrammingLanguageCategory { get; private set; }
        public Guid ProgrammingLanguageCategoryId { get; private set; }
        
        public virtual ICollection<CourseModuleEntity> CourseModules { get; private set; }
        public virtual ICollection<CourseThemeFieldPropertyEntity> ThemeFieldProperties { get; private set; }

        public void Update(string? name, string? description, string? avatarUrl, double? price, double? oldPrice,
            Guid? programmingLanguageCategoryId)
        {
            Name = string.IsNullOrEmpty(name) ? Name : name;
            Description = string.IsNullOrEmpty(description) ? Description : description;
            AvatarUrl = string.IsNullOrEmpty(avatarUrl) ? AvatarUrl : avatarUrl;
            Price = price ?? Price;
            OldPrice = oldPrice ?? OldPrice;
            ProgrammingLanguageCategoryId = programmingLanguageCategoryId ?? ProgrammingLanguageCategoryId;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }

        public bool GetIsFreeProperty()
        {
            return bool.Parse(ThemeFieldProperties
                .FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsFree)?
                .Value ?? string.Empty);
        }
    }
}