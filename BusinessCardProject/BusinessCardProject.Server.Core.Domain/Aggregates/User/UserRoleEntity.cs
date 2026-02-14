using BusinessCardProject.Server.Core.Domain.Commons;
using BusinessCardProject.Server.Core.Domain.Enums.User;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.User
{
    /// <summary>
    /// Отношение Many To Many между пользователем и ролью
    /// </summary>
    /// <param name="userProfileId"></param>
    /// <param name="userRole"></param>
    public class UserRoleEntity(Guid userProfileId, UserRoleEnum userRole) : Entity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual UserProfileEntity UserProfile { get; private set; }
        public Guid UserProfileId = userProfileId;
    
        /// <summary>
        /// Роль
        /// </summary>
        public UserRoleEnum UserRole { get; private set; } = userRole;
    }
}