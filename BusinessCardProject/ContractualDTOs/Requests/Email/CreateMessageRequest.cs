namespace Requests.Email
{
    public record CreateMessageRequest(
        IReadOnlyList<string> To,
        IReadOnlyList<string> Cc,
        IReadOnlyList<string> Bcc,
        string From,
        string DisplayName,
        string? ReplyTo,
        string? ReplyToName,
        string Subject,
        string Body,
        bool IsHtml
    );
}