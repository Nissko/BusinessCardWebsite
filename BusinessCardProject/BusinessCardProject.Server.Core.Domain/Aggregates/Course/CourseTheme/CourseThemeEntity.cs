using System.Globalization;
using BusinessCardProject.Server.Core.Domain.Commons;
using BusinessCardProject.Server.Core.Domain.Enums.Course;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme
{
    /// <summary>
    /// Название блока курса (категория)
    /// <example>База, ООП и тд</example>
    /// </summary>
    public class CourseThemeEntity : Entity
    {
        public CourseThemeEntity()
        {
            CourseModules = new HashSet<CourseModuleEntity>();
            ThemeRecommendations = new HashSet<ThemeRecommendationEntity>();
        }

        public CourseThemeEntity(string themeName, string themeDescription, Guid programmingLanguageId,
            TypeOfCourseEnum typeOfCourseId, int displayOrder, bool isActive, bool isFree) : this()
        {
            _themeName = themeName;
            _themeDescription = themeDescription;
            _typeOfCourseId = typeOfCourseId;
            _displayOrder = displayOrder;
            _isActive = isActive;
            _isFree = isFree;
            ProgrammingLanguageId = programmingLanguageId;
        }

        /// <summary>
        /// Название темы курса
        /// </summary>
        public string Name => _themeName;
        private string _themeName;

        /// <summary>
        /// Описание темы курса
        /// </summary>
        public string Description => _themeDescription;
        private string _themeDescription;

        /// <summary>
        /// Порядок отображения
        /// </summary>
        public int DisplayOrder => _displayOrder;
        private int _displayOrder;
    
        /// <summary>
        /// Нужно ли выводить на странице
        /// </summary>
        public bool IsActive => _isActive;
        private bool _isActive;

        /// <summary>
        /// Признак, который определяет, будет ли тема курса полностью платной
        /// </summary>
        public bool IsFree => _isFree;
        private bool _isFree;
    
        /// <summary>
        /// Тип курса
        /// </summary>
        public TypeOfCourseEnum TypeOfCourse => _typeOfCourseId;
        private TypeOfCourseEnum _typeOfCourseId;

        /// <summary>
        /// ЯП к которому принадлежит тема
        /// </summary>
        public virtual ProgrammingLanguageCourseEntity ProgrammingLanguages { get; private set; }
        public Guid ProgrammingLanguageId;

        #region virtual

        /// <summary>
        /// Коллекция категорий подготовок
        /// </summary>
        public virtual ICollection<CourseModuleEntity> CourseModules { get; private set; }

        /// <summary>
        /// Коллекция рекомендаций к курсу
        /// </summary>
        public virtual ICollection<ThemeRecommendationEntity> ThemeRecommendations { get; private set; }

        #endregion

        #region fucntions


        /// <summary>
        /// Обновление сущности
        /// </summary>
        public void Update(string param, string value)
        {
            if (string.IsNullOrEmpty(param))
                throw new ArgumentException("Имя параметра не может быть пустым", nameof(param));

            switch (param)
            {
                case nameof(Name):
                    _themeName = value;
                    break;

                case nameof(Description):
                    _themeDescription = value;
                    break;

                case nameof(DisplayOrder):
                    if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var displayOrder))
                    {
                        throw new ArgumentException($"Некорректное значение для {param}: '{value}'.");
                    }

                    _displayOrder = displayOrder;
                    break;

                case nameof(IsActive):
                    if (!bool.TryParse(value, out var isActive))
                    {
                        throw new ArgumentException($"Некорректное значение для {param}: '{value}'.");
                    }

                    _isActive = isActive;
                    break;

                case nameof(IsFree):
                    if (!bool.TryParse(value, out var isFree))
                    {
                        throw new ArgumentException($"Некорректное значение для {param}: '{value}'.");
                    }

                    _isFree = isFree;
                    break;

                default:
                    throw new ArgumentException($"Неизвестный параметр для обновления: '{param}'.");
            }
        }
    
        /// <summary>
        /// Метод для добавления модуля
        /// </summary>
        public void AddModule(CourseModuleEntity module)
        {
            CourseModules.Add(module);
        }

        #endregion
    }
}