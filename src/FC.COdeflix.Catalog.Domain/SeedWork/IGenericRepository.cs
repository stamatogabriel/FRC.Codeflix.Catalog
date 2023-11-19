using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAgreggate> : IRepository
{
    public Task Insert(TAgreggate aggregate, CancellationToken cancellationToken);
    public Task<TAgreggate> Get(Guid id, CancellationToken cancellationToken);
}
