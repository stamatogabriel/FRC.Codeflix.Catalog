namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
public class SearchInput
{
    public int Page { get; private set; }
    public int PerPage { get; private set; }
    public string Search { get; private set; }
    public string OrderBy { get; private set; }
    public SearchOrder Order { get; private set; }

    public SearchInput(int page, int perPage, string search, string orderBy, SearchOrder order)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
    }

}
