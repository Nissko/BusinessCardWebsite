using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task<string> CreateRecord(Guid userId, CancellationToken ct = default);
        Task<PasswordResetEntity?> GetValidRecord(Guid userId, string token, CancellationToken ct = default);
        Task MarkAsUsed(Guid userId, string token, CancellationToken ct = default);
        Task RevokeAllForUser(Guid userId, CancellationToken ct = default);
    }
}