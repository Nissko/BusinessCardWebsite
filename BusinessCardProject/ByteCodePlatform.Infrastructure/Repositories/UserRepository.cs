using ByteCodePlatform.Application.Common.Interfaces;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Extensions;
using Dtos.DTO.User;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.User;

namespace ByteCodePlatform.Infrastructure.Repositories
{
    public class UserRepository(IByteCodeCoreDbContext context) : IUserRepository
    {
        private readonly IByteCodeCoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

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
                       throw new("User not found");
            var newAuthor = new AuthorEntity(SystemClock.Instance.GetCurrentInstant(), null, null, request.UserId);
            
            _context.Author.Add(newAuthor);
            //TODO:разрешить через AuthService
            //user.SetAuthor(true);
            _context.User.Update(user);
            
            await _context.SaveChangesAsync(CancellationToken.None);

            var response = new UserAuthorDto(newAuthor.Id, user.GetUserCoreDto());
            return response;
        }
    }
}