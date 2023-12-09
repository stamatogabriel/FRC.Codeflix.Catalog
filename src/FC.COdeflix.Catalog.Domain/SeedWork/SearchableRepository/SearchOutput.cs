namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
public class SearchOutput<TAgreggate>
    where TAgreggate : AggregateRoot
{
    public int CurrentPage { get; private set; }
    public int PerPage { get; private set; }
    public int Total { get; private set; }
    public IReadOnlyList<TAgreggate> Items { get; private set; }

    public SearchOutput(int currentPage, int perPage, int total, IReadOnlyList<TAgreggate> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
