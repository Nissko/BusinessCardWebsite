using ByteCodePlatform.Application.Application.Extensions;
using Dtos.DTO.User;
using UserService.Proto;

namespace ByteCodePlatform.API.ProtoMappers.Course.Users
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
                DeletedAt = dto.DeletedAt?.ToTimestamp()
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