namespace FC.Codeflix.Catalog.Domain.SeedWork;
public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}
