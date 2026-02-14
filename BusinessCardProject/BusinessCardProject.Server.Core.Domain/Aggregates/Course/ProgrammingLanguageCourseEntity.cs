using BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme;
using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course
{
    /// <summary>
    /// ЯП для изучения
    /// </summary>
    public class ProgrammingLanguageCourseEntity : Entity
    {
        public ProgrammingLanguageCourseEntity()
        {
            CourseThemes = new HashSet<CourseThemeEntity>();
        }
    
        public ProgrammingLanguageCourseEntity(string name, int countSelectedUser) : this()
        {
            _name = name;
            _countSelectedUser = countSelectedUser;
        }

        #region Public Fields

        /// <summary>
        /// Название яп
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Сколько человек выбрали этот ЯП
        /// <example>Для статистики</example>
        /// </summary>
        public int CountSelectedUser => _countSelectedUser;

        #endregion

        #region Private properties

        /// <summary>
        /// Название яп
        /// </summary>
        private string _name;

        /// <summary>
        /// Сколько человек выбрали этот ЯП
        /// <example>Для статистики</example>
        /// </summary>
        private int _countSelectedUser;

        #endregion
    
        #region virtual
    
        /// <summary>
        /// Коллекция категорий подготовок
        /// </summary>
        public virtual ICollection<CourseThemeEntity> CourseThemes { get; private set; }

        #endregion

        #region functions

        /// <summary>
        /// Метод для добавления категории
        /// </summary>
        public void AddTheme(CourseThemeEntity theme)
        {
            CourseThemes.Add(theme);
        }

        /// <summary>
        /// Изменение информации о ЯП
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name)
        {
            _name = name;
        }

        #endregion
    
    }
}