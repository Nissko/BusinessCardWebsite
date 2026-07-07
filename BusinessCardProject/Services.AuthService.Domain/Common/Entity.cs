namespace Services.AuthService.Domain.Common
{
    public class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}