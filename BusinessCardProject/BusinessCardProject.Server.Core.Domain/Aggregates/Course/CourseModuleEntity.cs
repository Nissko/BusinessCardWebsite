using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Модуль курса (подкатегория)
/// </summary>
public class CourseModuleEntity : Entity
{
    public CourseModuleEntity()
    {
        VideoCourses = new HashSet<VideoCourseEntity>();
    }
    
    public CourseModuleEntity(string name, string description, Guid courseThemeId) :  this()
    {
        _name = name;
        _description = description;
        _courseThemeId = courseThemeId;
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
    /// Тема которой принадлежит модуль
    /// </summary>
    public virtual CourseThemeEntity CourseTheme { get; private set; }
    private Guid _courseThemeId;
    
    #region virtual
    
    /// <summary>
    /// Коллекция видеокурсов
    /// </summary>
    public virtual ICollection<VideoCourseEntity> VideoCourses { get; private set; }

    #endregion
    
    #region fucntions

    /// <summary>
    /// Метод для добавления видеокурса
    /// </summary>
    public void AddVideoCourse(VideoCourseEntity videoCourse)
    {
        VideoCourses.Add(videoCourse);
    }

    #endregion
}