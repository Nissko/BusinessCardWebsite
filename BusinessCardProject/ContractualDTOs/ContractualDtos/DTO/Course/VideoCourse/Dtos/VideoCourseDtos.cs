namespace ContractualDtos.DTO.Course.VideoCourse.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record VideoCourseDtos(
    Guid Id,
    string Name,
    string Description,
    string ImgUrl,
    string DatePublished,
    double Price,
    int Discount,
    double Rate,
    bool IsFree,
    bool IsShow,
    int DisplayOrder,
    Guid CourseAuthorId,
    Guid CourseModuleId,
    string LinkOnYoutube,
    string LinkOnRutube,
    string LinkOnVkVideo)
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// Id Автора, которому принадлежит
    /// </summary>
    public Guid CourseAuthorId { get; init; } = CourseAuthorId;

    /// <summary>
    /// Id Модуля, которому относится
    /// </summary>
    public Guid CourseModuleId { get; init; } = CourseModuleId;

    /// <summary>
    /// Название 
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Описание видеокурса
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// Ссылка на изображение из хранилища
    /// </summary>
    public string ImgUrl { get; init; } = ImgUrl;

    /// <summary>
    /// Дата публикации видеокурса
    /// </summary>
    public string DatePublished { get; init; } = DatePublished;

    /// <summary>
    /// Цена
    /// </summary>
    public double Price { get; init; } = Price;

    /// <summary>
    /// Размер скидки в %
    /// </summary>
    public int Discount { get; init; } = Discount;

    /// <summary>
    /// Рейтинг
    /// </summary>
    public double Rate { get; init; } = Rate;

    /// <summary>
    /// Бесплатный/Платный
    /// </summary>
    public bool IsFree { get; init; } = IsFree;

    /// <summary>
    /// Будет ли выводиться на странице
    /// </summary>
    public bool IsShow { get; init; } = IsShow;

    /// <summary>
    /// Порядок отображения, если видеокурсов много
    /// </summary>
    public int DisplayOrder { get; init; } = DisplayOrder;

    /// <summary>
    /// Ссылка курса на YouTube
    /// </summary>
    public string LinkOnYoutube { get; init; } = LinkOnYoutube;

    /// <summary>
    /// Ссылка курса на RuTube
    /// </summary>
    public string LinkOnRutube { get; init; } = LinkOnRutube;

    /// <summary>
    /// Ссылка курса на VkVideo
    /// </summary>
    public string LinkOnVkVideo { get; init; } = LinkOnVkVideo;
}