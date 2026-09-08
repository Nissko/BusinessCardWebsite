using AuthorizationService.Proto;
using ByteCodePlatform.Application.Common.Interfaces.GrpcClients;
using ByteCodePlatform.Application.Common.ProtobufClientExtensions;
using DTOs.DTO.Pagination;
using DTOs.DTO.User;
using Grpc.Core;
using RequestModels.User;

namespace ByteCodePlatform.Application.Application.GrpcClients
{
    public class AuthServiceGrpcServiceClient(
        AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient authorizationService)
        : IAuthGrpcService
    {
        private readonly AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient
            _authorizationService =
                authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

        public async Task<bool> AddAuthorRole(Guid userId)
        {
            try
            {
                var addAuthorRole = await _authorizationService.AddAuthorRoleAsync(new AddAuthorRoleRequest
                {
                    UserId = userId.ToString()
                });

                return addAuthorRole.Success;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public async Task<PaginationDto<UserDto>> GetUsersInfoFromSearch(GetUsersSearchRequest request)
        {
            try
            {
                var usersInfoFromAuth = await _authorizationService.GetUsersFromSearchAsync(
                    new GetUsersFromSearchRequest
                    {
                        Page = request.Page,
                        PageSize = request.PageSize,
                        Search = request.Search,
                        SortBy = request.SortBy,
                        SortDirection = request.SortDirection
                    });

                return new PaginationDto<UserDto>(
                    usersInfoFromAuth.Items.ToUserDtoFromProtoList(),
                    usersInfoFromAuth.TotalCount
                );
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public async Task<AuthorUserInfo> GetAuthorUserInfo(Guid userId)
        {
            try
            {
                var user = await _authorizationService.GetUserAsync(new GetUserRequest { UserId = userId.ToString() });
                return new AuthorUserInfo(user.Name, user.Surname, user.UserAvatar);
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}