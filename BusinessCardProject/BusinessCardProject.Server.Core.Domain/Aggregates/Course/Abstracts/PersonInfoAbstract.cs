using BusinessCardProject.Server.Core.Domain.Commons;
using NodaTime;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;

/// <summary>
/// Абстрактный класс основной информации о пользователе
/// </summary>
public abstract class PersonInfoAbstract : Entity
{
    protected PersonInfoAbstract(string surname, string name, string patronymic, string email, string altName)
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        AltName = altName;
        Email = email;
        DateOfRegistered = SystemClock.Instance.GetCurrentInstant();
    }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; private set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Patronymic { get; private set; }

    /// <summary>
    /// Никнейм (альтернативное имя)
    /// </summary>
    public string AltName { get; private set; }

    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Дата и время регистрации
    /// </summary>
    public Instant DateOfRegistered { get; private set; }

    /// <summary>
    /// Дата и время удаления
    /// </summary>
    public Instant? DateOfDeletion { get; private set; }
    
    protected void SetDateOfDeletion(bool isSafeDelete)
    {
        if (isSafeDelete)
        {
            DateOfDeletion = SystemClock.Instance.GetCurrentInstant();
        }
        else
        {
            DateOfDeletion = null;
        }
    }

    public void Update(string surname, string name, string patronymic, string email, string altName)
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Email = email;
        AltName = altName;
    }
}