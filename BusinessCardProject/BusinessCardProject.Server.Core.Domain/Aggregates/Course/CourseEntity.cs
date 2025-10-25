using BusinessCardProject.Server.Core.Domain.Common;
using BusinessCardProject.Server.Core.Domain.Enums.Course;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Сущность курса
/// </summary>
public class CourseEntity : Entity
{
    public CourseEntity(Guid courseAuthorId, string courseName, string courseDescription, string courseImg,
        DateTime courseDatePublished, double coursePrice, TypeOfCourseEnum courseType, bool isFree = false)
    {
        _courseAuthorId = courseAuthorId;
        _courseName = courseName;
        _courseDescription = courseDescription;
        //TODO: сделать получения default.jpg, если нет изображения
        _courseImg = courseImg ?? "default.jpg";
        _courseDatePublished = courseDatePublished;
        _coursePrice = coursePrice;
        _courseRate = 0;
        _courseRateCount = 0;
        _isFree = isFree;
        _courseType = courseType;
    }

    #region Public Properties

    /// <summary>
    /// Автор курса
    /// </summary>
    public Guid AuthorId => _courseAuthorId;

    /// <summary>
    /// Название
    /// </summary>
    public string Name => _courseName;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description => _courseDescription;

    /// <summary>
    /// Изображение
    /// </summary>
    public string Img => _courseImg;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime DatePublished => _courseDatePublished;

    /// <summary>
    /// Цена
    /// </summary>
    public double Price => _coursePrice;

    /// <summary>
    /// Рейтинг
    /// </summary>
    public double Rate => _courseRate;

    /// <summary>
    /// Тип курса
    /// </summary>
    public string TypeOfCourse => _courseType.Name;

    #endregion

    #region Private Fields

    /// <summary>
    /// Автор курса
    /// </summary>
    private Guid _courseAuthorId;

    /// <summary>
    /// Название
    /// </summary>
    private string _courseName;

    /// <summary>
    /// Описание
    /// </summary>
    private string _courseDescription;

    /// <summary>
    /// Обложка курса
    /// </summary>
    private string _courseImg;

    /// <summary>
    /// Дата создания
    /// </summary>
    private DateTime _courseDatePublished;

    /// <summary>
    /// Цена курса
    /// TODO: Сделать старую цену и скидку(считаем локально)
    /// </summary>
    private double _coursePrice;

    /// <summary>
    /// Рейтинг курса(Если ему оставят отзыв,
    /// то дернется метод, который обновит rate у курса)
    /// </summary>
    private double _courseRate;

    /// <summary>
    /// Кол-во оценок (Всего)
    /// Для перерасчета оценок
    /// </summary>
    private int _courseRateCount;

    /// <summary>
    /// Признак, платный курс или нет
    /// </summary>
    private bool _isFree;

    /// <summary>
    /// Тип курса(видео/лекция)
    /// </summary>
    private TypeOfCourseEnum _courseType;

    #endregion

    #region Functions

    /// <summary>
    /// Является ли курс платным
    /// </summary>
    public string IsItPaidCourse()
    {
        return _isFree ? "Да" : "Нет";
    }

    #endregion
}