namespace BusinessCardProject.Server.Core.Domain.Common;

public abstract class Entity
{
    Guid _id;
    public virtual Guid Id
    {
        get
        {
            return _id;
        }
        protected set
        {
            _id = value;
        }
    }

    public Entity()
    {
        Id = Guid.NewGuid();
    }
}