using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task<bool> CreateRecordAsync(Guid userId, CancellationToken ct = default);
        Task<PasswordResetEntity?> GetValidRecordAsync(Guid userId, string token, CancellationToken ct = default);
        Task MarkAsUsedAsync(Guid userId, string token, CancellationToken ct = default);
        Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);
    }
}