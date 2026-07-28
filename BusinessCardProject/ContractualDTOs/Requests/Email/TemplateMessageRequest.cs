namespace Requests.Email
{
    public class TemplateMessageRequest(
        // Название шаблона
        string TemplateName,
        // Содержание письма
        string TemplateBody,
        // Данные для подставки
        EmailMessageRequest EmailMessageRequest
    );
}