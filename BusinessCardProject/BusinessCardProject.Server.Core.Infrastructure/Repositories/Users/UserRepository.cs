using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User;
using BusinessCardProject.Server.Core.Domain.Aggregates.User;
using BusinessCardProject.Server.Core.Domain.Enums.User;
using BusinessCardProject.Server.Core.Infrastructure.Security.Interface;
using ContractualDtos.DTO.User.UserProfile.Dtos;
using ContractualDtos.DTO.User.UserProfile.Requests;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly IProjectDbContext _context;
    private readonly IPasswordHash _passwordHash;

    public UserRepository(IProjectDbContext context, IPasswordHash passwordHash)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    }

    #region crud

    public async Task<List<UserProfileDtos>> GetAllAsync()
    {
        try
        {
            var data = await _context.UserProfile.ToListAsync();
            return GetUserProfileDto(data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool?> Create(CreateUserProfileRequestDto dto)
    {
        try
        {
            var hashPass = await _passwordHash.HashPassword(dto.Password);
            var newUserProfile = new UserProfileEntity(dto.Surname, dto.Name, dto.Patronymic, dto.Email,
                dto.AltName, hashPass);
            var newUserRole = new UserRoleEntity(newUserProfile.Id, UserRoleEnum.User);

            newUserProfile.AddRole(newUserRole);
            _context.UserProfile.Add(newUserProfile);
            await SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<UserProfileDtos?> Read(Guid id)
    {
        try
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            return userProfile == null ? null : GetUserProfileDto(userProfile);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<UserProfileDtos?> Update(UpdateUserProfileRequestDto dto)
    {
        try
        {
            var userProfile = await _context.UserProfile.FindAsync(dto.Id);
            if (userProfile == null) return null;
            if (userProfile.AltName != dto.AltName)
            {
                var checkNewAltName = await _context.UserProfile.AnyAsync(t => t.AltName == dto.AltName);
                if (checkNewAltName)
                {
                    throw new Exception("Такой никнейм уже занят");
                }
            }

            userProfile.Update(dto.Surname, dto.Name, dto.Patronymic, dto.Email, dto.AltName);
            _context.UserProfile.Update(userProfile);
            await SaveChanges();

            return GetUserProfileDto(userProfile);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> SafeDelete(Guid id)
    {
        try
        {
            var userProfile = await _context.UserProfile.FindAsync([id]);
            if (userProfile == null) return false;
            if (userProfile.IsBlocked) throw new Exception("Пользователь уже заблокирован");

            userProfile.DeactivateAccount();
            _context.UserProfile.Update(userProfile);
            await SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> RecoveryUserProfile(Guid id)
    {
        try
        {
            var userProfile = await _context.UserProfile.FindAsync([id]);
            if (userProfile == null) return false;
            if (!userProfile.IsActive && userProfile.IsBlocked)
            {
                userProfile.RecoveryAccount();

                _context.UserProfile.Update(userProfile);
                await SaveChanges();

                return true;
            }

            throw new Exception("Пользователь не был удален.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    public async Task<bool> Login(AuthUserDto dto)
    {
        var userProfile = await _context.UserProfile.FirstOrDefaultAsync(t => t.AltName == dto.Nickname);
        if (userProfile == null) return false;

        var result = await _passwordHash.VerifyPassword(userProfile.Password, dto.Password);
        return result;
    }

    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    /// Формирование DTO для return
    /// </summary>
    private static UserProfileDtos GetUserProfileDto(UserProfileEntity entity)
    {
        return new UserProfileDtos(
            entity.Id,
            entity.Surname,
            entity.Name,
            entity.Patronymic,
            entity.Email,
            entity.AltName,
            entity.DateOfRegistered.ToLocalString()
        );
    }

    /// <summary>
    /// Формирование DTOs для return
    /// </summary>
    private static List<UserProfileDtos> GetUserProfileDto(List<UserProfileEntity> entities)
    {
        DateTimeZone systemZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
    
        return entities.Select(e => 
        {
            return new UserProfileDtos(
                e.Id,
                e.Surname,
                e.Name,
                e.Patronymic,
                e.Email,
                e.AltName,
                e.DateOfRegistered.ToLocalString()
            );
        }).ToList();
    }
}