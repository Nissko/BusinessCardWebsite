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

        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }
    
        public Guid RoleId { get; set; }
    }
}