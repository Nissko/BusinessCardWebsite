using BusinessCardProject.Server.Core.Domain.Commons;
using BusinessCardProject.Server.Core.Domain.Enums.Course;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Название блока курса (категория)
/// <example>База, ООП и тд</example>
/// </summary>
public class CourseThemeEntity : Entity
{
    public CourseThemeEntity()
    {
        CourseModules = new HashSet<CourseModuleEntity>();
    }

    public CourseThemeEntity(string themeName, string themeDescription, Guid programmingLanguageId,
        TypeOfCourseEnum typeOfCourseId) : this()
    {
        _themeName = themeName;
        _themeDescription = themeDescription;
        _typeOfCourseId = typeOfCourseId;
        _programmingLanguageId = programmingLanguageId;
    }

    /// <summary>
    /// Название темы курса
    /// </summary>
    public string ThemeName => _themeName;
    private string _themeName;

    /// <summary>
    /// Описание темы курса
    /// </summary>
    public string ThemeDescription => _themeDescription;
    private string _themeDescription;

    /// <summary>
    /// Тип курса
    /// </summary>
    public TypeOfCourseEnum TypeOfCourse => _typeOfCourseId;
    private TypeOfCourseEnum _typeOfCourseId;

    /// <summary>
    /// ЯП к которому принадлежит тема
    /// </summary>
    public virtual ProgrammingLanguageCourseEntity ProgrammingLanguages { get; private set; }
    private Guid _programmingLanguageId;

    #region virtual

    /// <summary>
    /// Коллекция категорий подготовок
    /// </summary>
    public virtual ICollection<CourseModuleEntity> CourseModules { get; private set; }

    #endregion

    #region fucntions

    /// <summary>
    /// Обновление сущности
    /// </summary>
    public void Update(string name, string description, Guid typeOfCourseId,
        ProgrammingLanguageCourseEntity programmingLanguageId)
    {
        _themeName = name;
        _themeDescription = description;
        _typeOfCourseId = TypeOfCourseEnum.FromId(typeOfCourseId);
        ProgrammingLanguages = programmingLanguageId;
    }
    
    /// <summary>
    /// Обновление сущности
    /// </summary>
    public void Update(string name, string description, Guid typeOfCourseId)
    {
        _themeName = name;
        _themeDescription = description;
        _typeOfCourseId = TypeOfCourseEnum.FromId(typeOfCourseId);
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