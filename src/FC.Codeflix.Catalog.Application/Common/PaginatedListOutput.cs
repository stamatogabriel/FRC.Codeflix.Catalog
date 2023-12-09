namespace FC.Codeflix.Catalog.Application.Common;
public abstract class PaginatedListOutput<TOutputItem>
{
    public int CurrentPage { get; private set; }
    public int PerPage { get; private set; }
    public int Total { get; private set; }
    public IReadOnlyList<TOutputItem> Items { get; private set; }

    protected PaginatedListOutput(int currentPage, int perPage, int total, IReadOnlyList<TOutputItem> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
