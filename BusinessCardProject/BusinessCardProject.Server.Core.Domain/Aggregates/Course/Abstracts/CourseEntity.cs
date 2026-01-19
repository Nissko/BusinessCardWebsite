using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;

/// <summary>
/// Сущность курса
/// </summary>
public abstract class CourseEntity(
    Guid courseAuthorId,
    string courseName,
    string courseDescription,
    string courseImg,
    DateTime courseDatePublished,
    double coursePrice,
    int courseDiscount,
    bool isShow,
    int displayOrder,
    bool isFree = false)
    : Entity
{
    //TODO: сделать получения default.jpg, если нет изображения

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
    /// Скидка на курс
    /// </summary>
    public int Discount => _courseDiscount;

    /// <summary>
    /// Рейтинг
    /// </summary>
    public double Rate => _courseRate;

    /// <summary>
    /// Признак, бесплатный или платный курс
    /// </summary>
    public bool IsFree => _isFree;

    #endregion

    #region Private Fields

    /// <summary>
    /// Автор курса
    /// </summary>
    private Guid _courseAuthorId = courseAuthorId;

    /// <summary>
    /// Название
    /// </summary>
    private string _courseName = courseName;

    /// <summary>
    /// Описание
    /// </summary>
    private string _courseDescription = courseDescription;

    /// <summary>
    /// Обложка курса
    /// </summary>
    private string _courseImg = courseImg ?? "default.jpg";

    /// <summary>
    /// Дата создания
    /// </summary>
    private DateTime _courseDatePublished = courseDatePublished;

    /// <summary>
    /// Цена курса
    /// TODO: Сделать старую цену и скидку(считаем локально)
    /// </summary>
    private double _coursePrice = coursePrice;

    /// <summary>
    /// Скидка в %
    /// </summary>
    private int _courseDiscount = courseDiscount;

    /// <summary>
    /// Рейтинг курса(Если ему оставят отзыв,
    /// то дернется метод, который обновит rate у курса)
    /// </summary>
    private double _courseRate = 0;

    /// <summary>
    /// Кол-во оценок (Всего)
    /// Для перерасчета оценок
    /// </summary>
    private int _courseRateCount = 0;

    /// <summary>
    /// Признак, платный курс или нет
    /// </summary>
    private bool _isFree = isFree;

    #endregion

    #region Options

    public bool IsShow { get; private set; } = isShow;
    public int DisplayOrder { get; private set; } = displayOrder;

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