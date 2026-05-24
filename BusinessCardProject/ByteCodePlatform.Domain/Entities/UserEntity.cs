namespace ByteCodePlatform.Domain.Entities
{
    public class UserEntity
    {
        public UserEntity(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; private set; }
        
        public virtual AuthorEntity Author { get; private set; }
    }
}