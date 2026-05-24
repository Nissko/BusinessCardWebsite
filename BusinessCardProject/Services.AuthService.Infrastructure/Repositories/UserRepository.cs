using Dtos.DTO.User;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.User;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;
using Services.AuthService.Domain.Entities;
using Services.AuthService.Domain.Enums;
using Services.AuthService.Domain.Extensions;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class UserRepository(IAuthDbContext context, IUserCoreGrpcService userCoreGrpcService) : IUserRepository
    {
        private readonly IAuthDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IUserCoreGrpcService _userCoreGrpcService = userCoreGrpcService;

        public async Task<bool> CreateUser(CreateUserRequest request)
        {
            if (_context.User.Any(x => x.Email == request.Email))
                throw new("User email already exists");
            if (_context.User.Any(x => x.NickName == request.NickName))
                throw new("User nickname already exists");

            var user = new UserEntity(request.Surname, request.Name, request.NickName, request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12),
                SystemClock.Instance.GetCurrentInstant());
            user.UserRoles.Add(new UserRolesEntity(user.Id, UserRoleEnum.User.Id));

            try
            {
                await _userCoreGrpcService.CreateUser(user.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
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

        public async Task<UserDto> GetUser(Guid userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId) ??
                       throw new("User not found");
            return user.GetUserDto();
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email)
                       ?? throw new("User not found");
            return user.GetUserDto();
        }

        public async Task<bool> VerifyPassword(Guid userId, string plainPassword)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId)
                       ?? throw new("User not found");
            return BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash);
        }

        public async Task<List<string>> GetRoles(Guid userId)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == userId) ?? throw new Exception("User not found");
            return user.UserRoles.Select(role => role.RoleId.ToString()).ToList();
        }
    }
}