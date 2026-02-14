using BusinessCardProject.Server.Core.Domain.Aggregates.User;

namespace ContractualDtos.DTO.User.UserProfile.Requests
{
    public record AddNewUserRoleRequestDto(UserProfileEntity UserProfile, Guid RoleId)
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public UserProfileEntity UserProfile { get; init; } = UserProfile;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Guid RoleId { get; init; } = RoleId;
    }
}