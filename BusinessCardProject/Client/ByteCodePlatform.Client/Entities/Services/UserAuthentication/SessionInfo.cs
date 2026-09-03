namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public record SessionInfo(string TokenId, DateTimeOffset CreatedAt, DateTimeOffset ExpiresAt, string UserAgent);
}