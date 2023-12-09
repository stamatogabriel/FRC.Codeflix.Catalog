namespace FC.Codeflix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAgreggate> : IRepository
    where TAgreggate : AggregateRoot
{
    public Task Insert(TAgreggate aggregate, CancellationToken cancellationToken);
    public Task<TAgreggate> Get(Guid id, CancellationToken cancellationToken);
    public Task Delete(TAgreggate agreggate, CancellationToken cancellationToken);
    public Task Update(TAgreggate agreggate, CancellationToken cancellationToken);
}
