namespace ContractualDtos.DTO.User.UserProfile.Dtos
{
    public record UserRoleDto(string RoleName)
    {
        /// <summary>
        /// Название роли пользователя
        /// </summary>
        public string RoleName { get; init; } = RoleName;
    };
}