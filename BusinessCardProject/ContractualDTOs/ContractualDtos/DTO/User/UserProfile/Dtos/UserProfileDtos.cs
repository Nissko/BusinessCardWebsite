using BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting;

namespace ContractualDtos.DTO.User.UserProfile.Dtos
{
    /// <summary>
    /// DTO для возврата объекта
    /// </summary>
    public record UserProfileDtos(
        Guid Id,
        string Surname,
        string Name,
        string Patronymic,
        string Email,
        string AltName,
        string CreatedOn,
        UserSetting UserSetting,
        List<UserRoleDto> UserRoles);
}