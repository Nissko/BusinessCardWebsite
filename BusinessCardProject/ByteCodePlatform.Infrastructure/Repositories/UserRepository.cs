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

        public async Task<bool> CreateUser(CreateUserRequest request)
        {
            if (_context.User.Any(x => x.Email == request.Email)) 
                throw new("User email already exists");
            if (_context.User.Any(x => x.NickName == request.NickName))
                throw new("User nickname already exists");
            
            var user = new UserEntity(request.Surname, request.Name, request.NickName, request.Email,
                SystemClock.Instance.GetCurrentInstant());
            
            _context.User.Add(user);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }

        public async Task<bool> UpdateUser(UpdateUserRequest request)
        {
            if (_context.User.Any(x => x.Email == request.Email)) 
                throw new("User email already exists");
            if (_context.User.Any(x => x.NickName == request.NickName))
                throw new("User nickname already exists");

            var user = _context.User.FirstOrDefault(x => x.Id == request.Id) ?? throw new("User not found");
            user.UpdateUser(request.Surname, request.Name, request.NickName, request.Email,
                SystemClock.Instance.GetCurrentInstant());
            
            _context.User.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }

        public async Task<UserAuthorDto> CreateAuthorUser(CreateAuthorRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == request.UserId) ??
                       throw new("User not found");
            var newAuthor = new AuthorEntity(SystemClock.Instance.GetCurrentInstant(), null, null, request.UserId);
            
            _context.Author.Add(newAuthor);
            user.SetAuthor(true);
            _context.User.Update(user);
            
            await _context.SaveChangesAsync(CancellationToken.None);

            var response = new UserAuthorDto(newAuthor.Id, user.GetUserDto());
            return response;
        }

        public async Task<UserDto> GetUser(Guid userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId) ??
                       throw new("User not found");

            return user.GetUserDto();
        }
    }
}