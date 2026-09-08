using ByteCodePlatform.Application.Common.Interfaces;
using ByteCodePlatform.Application.Common.Interfaces.GrpcClients;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Extensions;
using DTOs.DTO.Pagination;
using DTOs.DTO.User;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using RequestModels.User;

namespace ByteCodePlatform.Infrastructure.Repositories
{
    public class UserRepository(IByteCodeCoreDbContext context, IAuthGrpcService authGrpcService) : IUserRepository
    {
        private readonly IByteCodeCoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        private readonly IAuthGrpcService _authGrpcService = authGrpcService
                                                             ?? throw new ArgumentNullException(
                                                                 nameof(authGrpcService));

        public async Task<bool> CreateUser(Guid userId)
        {
            var newUser = new UserEntity(userId);

            _context.User.Add(newUser);
            await _context.SaveChangesAsync(CancellationToken.None);

            return true;
        }

        public async Task<UserAuthorDto> CreateAuthorUser(CreateAuthorRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserId == request.UserId) ??
                       throw new("Пользователь не найден");
            var userAuthService = await _authGrpcService.GetAuthorUserInfo(user.UserId);

            var newAuthor = new AuthorEntity(userAuthService.Name, userAuthService.Surname, "", userAuthService.AvatarId,
                SystemClock.Instance.GetCurrentInstant(), null, null, request.UserId);
            var addRole = await _authGrpcService.AddAuthorRole(newAuthor.UserId);

            if (!addRole) throw new Exception("Не удалось добавить роль");

            _context.Author.Add(newAuthor);
            _context.User.Update(user);

            await _context.SaveChangesAsync(CancellationToken.None);

            var response = new UserAuthorDto(newAuthor.Id, user.GetUserCoreDto(),
                newAuthor.Name, newAuthor.Surname, newAuthor.AboutUs, newAuthor.AvatarId);

            return response;
        }

        public async Task<PaginationDto<UserDto>> GetUsersFromSearch(GetUsersSearchRequest request)
        {
            var usersInfo = await _authGrpcService.GetUsersInfoFromSearch(new GetUsersSearchRequest(request.Page,
                request.PageSize, request.Search, request.SortBy, request.SortDirection));
            return usersInfo;
        }

        public async Task<bool> UpdateAuthorAvatar(Guid userId, string avatarId)
        {
            var author = await _context.Author.FirstOrDefaultAsync(au => au.UserId == userId)
                         ?? throw new Exception("Автор не найден");
            author.UpdateAuthorAvatar(avatarId);
            
            _context.Author.Update(author);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }
    }
}