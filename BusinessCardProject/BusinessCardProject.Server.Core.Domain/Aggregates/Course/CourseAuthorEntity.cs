using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Сущность автора курса
/// </summary>
public class CourseAuthorEntity : Entity
{
    public CourseAuthorEntity()
    {
        VideoCourses = new HashSet<VideoCourseEntity>();
    }

    public CourseAuthorEntity(string authorName, string authorSurname, string authorPatronymic,
        string? authorNickName = null) : this()
    {
        _authorName = authorName;
        _authorSurname = authorSurname;
        _authorPatronymic = authorPatronymic;
        _authorNickName = authorNickName;
    }

    #region Public Properties

    /// <summary>
    /// Имя
    /// </summary>
    public string Name => _authorName;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname => _authorSurname;

    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronymic => _authorPatronymic;

    /// <summary>
    /// Альтернативное имя
    /// </summary>
    public string Nickname => GetNickname();

    /// <summary>
    /// ФИО
    /// Формат: Иванов И. И.
    /// </summary>
    public string FullName => GetFullName();

    #endregion

    #region Private Fields

    /// <summary>
    /// Имя автора
    /// </summary>
    private string _authorName;

    /// <summary>
    /// Фамилия автора
    /// </summary>
    private string _authorSurname;

    /// <summary>
    /// Отчество автора
    /// </summary>
    private string _authorPatronymic;

    /// <summary>
    /// Никнейм автора
    /// <remarks>Если автор хочет скрыть ФИО</remarks>
    /// </summary>
    private string? _authorNickName;
    
    //TODO: Добавить поле с Guid из таблицы Users

    #endregion

    #region virtual

    /// <summary>
    /// Коллекция видеокурсов
    /// </summary>
    public virtual ICollection<VideoCourseEntity> VideoCourses { get; private set; }

    #endregion

    #region Functions

    #region fucntions

    /// <summary>
    /// Метод для добавления видеокурса
    /// </summary>
    public void AddVideoCourse(VideoCourseEntity videoCourse)
    {
        VideoCourses.Add(videoCourse);
    }

    #endregion
    
    /// <summary>
    /// ФИО автора
    /// </summary>
    /// <returns>Иванов И. И.</returns>
    private string GetFullName()
    {
        return $"{_authorPatronymic}  {_authorName.First()}. {_authorSurname.First()}.";
    }

    /// <summary>
    /// Получить Ник автора
    /// </summary>
    private string GetNickname()
    {
        /*TODO: Переделать на кастомное исключение*/
        return _authorNickName is null
            ? throw new ArgumentNullException(_authorNickName)
            : $"{_authorNickName}";
    }

    #endregion
}