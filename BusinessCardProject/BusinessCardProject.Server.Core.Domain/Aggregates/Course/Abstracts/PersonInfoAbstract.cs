using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;

/// <summary>
/// Абстрактный класс основной информации о пользователе
/// </summary>
public abstract class PersonInfoAbstract(
    string surname,
    string name,
    string patronymic,
    string email,
    string altName,
    DateTime dateOfRegistered,
    DateTime? dateOfDeletion)
    : Entity
{
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname => surname;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name => name;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Patronymic => patronymic;

    /// <summary>
    /// Никнейм (альтернативное имя)
    /// </summary>
    public string AltName => altName;

    /// <summary>
    /// Почта
    /// </summary>
    public string Email => email;

    /// <summary>
    /// Дата и время регистрации
    /// </summary>
    public string DateOfRegistered => dateOfRegistered.ToShortDateString();

    /// <summary>
    /// Дата и время удаления
    /// </summary>
    public string DateOfDeletion => dateOfDeletion?.ToShortDateString() ?? "";
}