namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
public interface ISearchableRepository<TAgreggate>
    where TAgreggate : AggregateRoot
{
    Task<SearchOutput<TAgreggate>> Search(SearchInput input, CancellationToken cancellationToken);
}
