using DTOs.DTO.Pagination;
using DTOs.DTO.User;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using RequestModels.User;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;
using Services.AuthService.Domain.Entities;
using Services.AuthService.Domain.Enums;
using Services.AuthService.Domain.Extensions;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class UserRepository(
        IAuthDbContext context,
        ICoreGrpcServiceClient coreGrpcServiceClient,
        IAccountVerificationRepository accVerifRepository) : IUserRepository
    {
        private readonly IAuthDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        private readonly IAccountVerificationRepository _accVerifRepository = accVerifRepository
                                                                              ?? throw new ArgumentNullException(
                                                                                  nameof(accVerifRepository));

        public async Task<bool> CreateUser(CreateUserRequest request)
        {
            var now = SystemClock.Instance.GetCurrentInstant();

            var emailExists = await _context.User
                .AsNoTracking()
                .AnyAsync(x => x.Email == request.Email, CancellationToken.None)
                .ConfigureAwait(false);

            if (emailExists)
                throw new Exception("Электронная почта пользователя уже существует");

            var nickNameExists = await _context.User
                .AsNoTracking()
                .AnyAsync(x => x.NickName == request.NickName, CancellationToken.None)
                .ConfigureAwait(false);

            if (nickNameExists)
                throw new Exception("Псевдоним пользователя уже существует");

            var user = new UserEntity(
                request.Surname,
                request.Name,
                request.NickName,
                request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12),
                now);

            user.UserRoles.Add(new UserRolesEntity(user.Id, UserRoleEnum.User.Id));

            _context.User.Add(user);
            await _context.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);

            var addUserToCoreTask = coreGrpcServiceClient.CreateUser(user.Id);
            var createRecordVerificationTask = _accVerifRepository.CreateRecord(user.Id);

            await Task.WhenAll(addUserToCoreTask, createRecordVerificationTask).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> UpdateUser(UpdateUserRequest request)
        {
            if (_context.User.Any(x => x.Email == request.Email))
                throw new("Электронная почта пользователя уже существует");
            if (_context.User.Any(x => x.NickName == request.NickName))
                throw new("Псевдоним пользователя уже существует");

            var user = await _context.User.FindAsync(request.Id) ?? throw new("Пользователь не найден");
            user.UpdateUser(request.Surname, request.Name, request.NickName, request.Email,
                SystemClock.Instance.GetCurrentInstant());

            _context.User.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);

            return true;
        }

        public async Task<bool> UpdateUserAvatar(Guid userId ,string avatarId)
        {
            var user = await _context.User.FindAsync(userId) ?? throw new Exception("Пользователь не найден");
            user.UpdateAvatar(avatarId);
            
            _context.User.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }

        public async Task<UserDto> GetUser(Guid userId)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new("Пользователь не найден");
            return user.GetUserDto();
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email)
                       ?? throw new("Пользователь не найден");
            return user.GetUserDto();
        }

        public async Task<bool> VerifyPassword(Guid userId, string plainPassword)
        {
            var user = await _context.User.FindAsync([userId])
                       ?? throw new("Пользователь не найден");
            return BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash);
        }

        public async Task<bool> CheckVerificationAcc(Guid userId)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new Exception("Пользователь не найден");
            return user.VerifyMail;
        }

        public async Task<bool> AddAuthorRole(Guid userId)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new("Пользователь не найден");

            if (user.UserRoles.Any(x => x.RoleId == UserRoleEnum.Author.Id))
            {
                throw new("Пользователь уже является автором");
            }

            user.SetAuthor(true);
            var newRole = new UserRolesEntity(user.Id, UserRoleEnum.Author.Id);

            _context.UserRole.Add(newRole);
            _context.User.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);

            return true;
        }

        public async Task<PaginationDto<UserDto>> GetUsersFromSearch(GetUsersSearchRequest request)
        {
            var query = _context.User.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = $"%{request.Search}%";
                query = query.Where(u =>
                    EF.Functions.ILike(u.Name, searchTerm) ||
                    EF.Functions.ILike(u.Surname, searchTerm) ||
                    EF.Functions.ILike(u.NickName, searchTerm) ||
                    EF.Functions.ILike(u.Email, searchTerm)
                );
            }

            var isDescending = request.SortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
            query = request.SortBy?.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(u => u.Name)
                    : query.OrderBy(u => u.Name),
                "surname" => isDescending
                    ? query.OrderByDescending(u => u.Surname)
                    : query.OrderBy(u => u.Surname),
                "nickname" => isDescending
                    ? query.OrderByDescending(u => u.NickName)
                    : query.OrderBy(u => u.NickName),
                "email" => isDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),
                "createdat" => isDescending
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),
                _ => isDescending
                    ? query.OrderByDescending(u => u.Id)
                    : query.OrderBy(u => u.Id)
            };

            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var skip = (page - 1) * pageSize;

            query = query.Skip(skip).Take(pageSize);
            var users = await query
                .Select(u => new UserDto
                (
                    u.Id,
                    u.Surname,
                    u.Name,
                    u.NickName,
                    u.Email,
                    u.IsAuthor,
                    u.CreatedAt,
                    u.UpdatedAt,
                    u.DeletedAt,
                    u.VerifyMail,
                    u.AvatarId
                ))
                .ToListAsync();

            return new PaginationDto<UserDto>(users, users.Count);
        }

        public async Task<List<string>> GetRoles(Guid userId)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new Exception("Пользователь не найден");
            return user.UserRoles.Select(role => role.RoleId.ToString()).ToList();
        }

        public async Task<bool> UpdatePassword(Guid userId, string newPassword)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new("Пользователь не найден");
            
            user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12));
            
            _context.User.Update(user);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }
    }
}