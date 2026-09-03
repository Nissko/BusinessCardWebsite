using AuthorizationService.Proto;
using ByteCodePlatform.Application.Application.Extensions;
using Dtos.DTO.User;

namespace ByteCodePlatform.Application.Common.ProtobufClientExtensions
{
    public static class UserToGrpcResponse
    {
        private static UserDto ToUserDtoFromProto(this UserInfoResponse proto)
        {
            return new UserDto(
                Id: Guid.Parse(proto.UserId),
                Surname: proto.Surname,
                Name: proto.Name,
                NickName: proto.Nickname,
                Email: proto.Email,
                IsAuthor: proto.IsAuthor,
                CreatedAt: proto.CreatedAt.ToInstant(),
                UpdatedAt: proto.UpdatedAt?.ToInstant(),
                DeletedAt: proto.DeletedAt?.ToInstant(),
                IsVerified: proto.IsVerified,
                UserAvatar: proto.UserAvatar
            );
        }

        public static List<UserDto> ToUserDtoFromProtoList(this IEnumerable<UserInfoResponse> protos)
        {
            return protos.Select(ToUserDtoFromProto).ToList();
        }
    }
}