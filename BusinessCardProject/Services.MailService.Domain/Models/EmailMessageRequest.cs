namespace Services.MailService.Domain.Models
{
    public record EmailMessageRequest(
        // Список основных получателей
        IReadOnlyList<string> To,
        // Список получателей копии письма
        IReadOnlyList<string> Cc,
        // Список получателей получающие скрытую копию письма
        IReadOnlyList<string> Bcc,
        // Отправитель
        string From,
        // Имя отправителя
        string DisplayName,
        // Кому будут отправляться ответы
        string? ReplyTo,
        // Отображаемое имя для адреса ответа
        string? ReplyToName,
        // Тема
        string Subject,
        // Тело письма
        string Body,
        bool IsHtml
    );
}