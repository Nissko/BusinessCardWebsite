using BusinessCardProject.Server.Core.Domain.Common;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// ЯП для изучения
/// </summary>
public class CategoryEntity : Entity
{
    public CategoryEntity()
    {
        _courses = new List<CourseEntity>();
    }
    
    /// <summary>
    /// Название категории
    /// </summary>
    private string _name;
    
    /// <summary>
    /// Курсы категории
    /// </summary>
    private ICollection<CourseEntity> _courses;
}