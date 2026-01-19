using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// ЯП для изучения
/// </summary>
internal class ProgrammingLanguageCourseEntity : Entity
{
    public ProgrammingLanguageCourseEntity(string name, int countSelectedUser)
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

    #region Private Fields

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
}