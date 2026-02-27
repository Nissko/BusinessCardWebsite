using BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme;
using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course
{
    /// <summary>
    /// Модуль курса (подкатегория)
    /// </summary>
    public class CourseModuleEntity : Entity
    {
        public CourseModuleEntity()
        {
            VideoCourses = new HashSet<VideoCourseEntity>();
        }
    
        public CourseModuleEntity(string name, string description, Guid courseThemeId, bool isShow, int displayOrder) :  this()
        {
            _name = name;
            _description = description;
            _isShow = isShow;
            _displayOrder = displayOrder;
            CourseThemeId = courseThemeId;
        }
    
        /// <summary>
        /// Название модуля
        /// </summary>
        public string Name => _name;
        private string _name;

        /// <summary>
        /// Описание модуля
        /// </summary>
        public string Description => _description;
        private string _description;

        /// <summary>
        /// Нужно ли выводить
        /// </summary>
        public bool IsShow => _isShow;
        private bool _isShow;
        
        /// <summary>
        /// 
        /// </summary>
        public int DisplayOrder => _displayOrder;
        private int _displayOrder;

        /// <summary>
        /// Тема которой принадлежит модуль
        /// </summary>
        public virtual CourseThemeEntity CourseTheme { get; private set; }
        public Guid CourseThemeId;
    
        #region virtual
    
        /// <summary>
        /// Коллекция видеокурсов
        /// </summary>
        public virtual ICollection<VideoCourseEntity> VideoCourses { get; private set; }

        #endregion
    
        #region fucntions

        public void Update(string name, string description, CourseThemeEntity courseTheme)
        {
            _name = name;
            _description = description;
            CourseTheme = courseTheme;
        }
    
        public void Update(string param, string value)
        {
            if (string.IsNullOrEmpty(param))
                throw new ArgumentException("Имя параметра не может быть пустым", nameof(param));

            switch (param)
            {
                case nameof(Name):
                    _name = value;
                    break;

                case nameof(Description):
                    _description = value;
                    break;
                
                case nameof(DisplayOrder):
                    _displayOrder = int.Parse(value);
                    break;
                
                case nameof(IsShow):
                    _isShow = bool.Parse(value);
                    break;
                
                default:
                    throw new ArgumentException($"Неизвестный параметр для обновления: '{param}'.");
            }
        }
    
        /// <summary>
        /// Метод для добавления видеокурса
        /// </summary>
        public void AddVideoCourse(VideoCourseEntity videoCourse)
        {
            VideoCourses.Add(videoCourse);
        }

        #endregion
    }
}