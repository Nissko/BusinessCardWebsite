using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Модуль курса (подкатегория)
/// </summary>
internal class CourseModuleEntity : Entity
{
    public CourseModuleEntity(string name, string description, Guid courseThemeId)
    {
        _name = name;
        _description = description;
        _courseThemeId = courseThemeId;
    }
    
    /// <summary>
    /// Название модуля
    /// </summary>
    private string _name;

    /// <summary>
    /// Описание модуля
    /// </summary>
    private string _description;

    /// <summary>
    /// Id модуля к которому принадлежит тема
    /// </summary>
    private Guid _courseThemeId;
}