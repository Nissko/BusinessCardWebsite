using ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog;

namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Interfaces
{
    public interface IEntityUpdateService
    {
        Task<bool> UpdateFieldAsync(TypeOfEntityType entityType, Guid id, string fieldName, object? value);
    }
}