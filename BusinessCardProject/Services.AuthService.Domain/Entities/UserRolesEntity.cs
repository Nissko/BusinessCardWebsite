using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class UserRolesEntity : Entity
    {
        public UserRolesEntity(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; private set; }
        public virtual UserEntity User { get; private set; }
        public Guid RoleId { get; private set; }
    }
}