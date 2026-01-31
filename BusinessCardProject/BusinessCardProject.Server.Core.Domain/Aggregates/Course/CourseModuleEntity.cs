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
    
    public void Update(string name, string description)
    {
        _name = name;
        _description = description;
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