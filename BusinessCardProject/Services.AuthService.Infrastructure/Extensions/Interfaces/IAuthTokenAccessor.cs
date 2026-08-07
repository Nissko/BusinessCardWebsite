namespace Services.AuthService.Infrastructure.Extensions.Interfaces
{
    public interface IAuthTokenAccessor
    {
        /// <summary>
        /// Получение токена авторизации
        /// </summary>
        string? GetToken();
    }
}