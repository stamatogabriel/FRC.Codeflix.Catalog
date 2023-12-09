using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.Application.Common;
public abstract class PaginatedListInput
{
    public int Page { get; private set; }
    public int PerPage { get; private set; }
    public string Search { get; private set; }
    public string Sort { get; private set; }
    public SearchOrder Dir { get; private set; }

    public PaginatedListInput(int page, int perPage, string search, string sort, SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        Sort = sort;
        Dir = dir;
    }


}
