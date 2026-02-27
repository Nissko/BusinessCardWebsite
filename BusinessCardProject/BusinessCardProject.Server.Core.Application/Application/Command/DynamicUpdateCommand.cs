using BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;

namespace BusinessCardProject.Server.Core.Application.Application.Command;

/// <summary>
/// Динамическое изменение сущностей
/// </summary>
public class DynamicUpdateCommand : IRequest<bool>
{
    public DynamicUpdateCommand(Guid id, string entityApiName, string fieldApiName, string value)
    {
        Id = id;
        EntityApiName = entityApiName;
        FieldApiName = fieldApiName;
        Value = value;
    }

    /// <summary>
    /// Ид записи в таблице
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Изменяемая сущность
    /// </summary>
    public string EntityApiName { get; set; }

    /// <summary>
    /// Изменяемое поле
    /// </summary>
    public string FieldApiName { get; set; }

    /// <summary>
    /// Значение
    /// </summary>
    public string Value { get; set; }
}