namespace ContractualDtos.DTO.User.UserProfile.Dtos
{
    public record AuthUserDto(string Nickname, string Password)
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Nickname { get; init; } = Nickname;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; init; } = Password;
    };
}