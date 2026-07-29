using Dtos.DTO.User;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Domain.Extensions
{
    public static class ConvertEntityToDtoExtensions
    {
        #region User

        public static UserDto GetUserDto(this UserEntity e)
        {
            return new(
                e.Id,
                e.Surname,
                e.Name,
                e.NickName,
                e.Email,
                e.IsAuthor,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.DeletedAt ?? null,
                e.VerifyMail
            );
        }

        public static List<UserDto> GetUserDto(this List<UserEntity> en)
        {
            return en.Select(e => new UserDto(
                e.Id,
                e.Surname,
                e.Name,
                e.NickName,
                e.Email,
                e.IsAuthor,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.DeletedAt ?? null,
                e.VerifyMail
            )).ToList();
        }
        
        #endregion
    }
}