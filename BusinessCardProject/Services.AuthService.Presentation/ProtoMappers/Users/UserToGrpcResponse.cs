using AuthorizationService.Proto;
using Dtos.DTO.User;
using Services.AuthService.Application.Application.Extensions;

namespace Services.AuthService.Presentation.ProtoMappers.Users
{
    public static class UserToGrpcResponse
    {
        private static UserInfoResponse ToProtoUsersFromSearchInfo(this UserDto dto)
        {
            return new UserInfoResponse
            {
                UserId = dto.Id.ToString(),
                Surname = dto.Surname,
                Name = dto.Name,
                Nickname = dto.NickName,
                Email = dto.Email,
                IsAuthor = dto.IsAuthor,
                CreatedAt = dto.CreatedAt.ToTimestamp(),
                UpdatedAt = dto.UpdatedAt?.ToTimestamp(),
                DeletedAt = dto.DeletedAt?.ToTimestamp(),
                IsVerified = dto.IsVerified
            };
        }
        
        public static GetUsersFromSearchResponse ToProtoUsersFromSearchInfoList(
            this List<UserDto> dtos)
        {
            var response = new GetUsersFromSearchResponse
            {
                TotalCount = dtos.Count
            };
            response.Items.AddRange(dtos.Select(ToProtoUsersFromSearchInfo));
            return response;
        }
    }
}