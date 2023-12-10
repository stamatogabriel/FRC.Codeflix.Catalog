using FC.Codeflix.Catalog.Infra.Data.EF;
using Repository = FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task Insert()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
        
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task Get()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetValidCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotfound))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task GetThrowIfNotfound()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        Guid exampleId = Guid.NewGuid();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

        var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleId}' not found");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task Update()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetValidCategory();
        var newCategoryValues = _fixture.GetValidCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
        
        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task Delete()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetValidCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task SearchReturnListAndTotal()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);

        foreach(Category outputItem in output.Items)
        {
            var exempleItem = exampleCategoriesList.Find(category => category.Id == outputItem.Id);

            exempleItem.Should().NotBeNull();
            exempleItem.Name.Should().Be(outputItem.Name);
            exempleItem.Description.Should().Be(outputItem.Description);
            exempleItem.IsActive.Should().Be(outputItem.IsActive);
            exempleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnEmptyList))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    public async Task SearchReturnEmptyList()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnPaginated))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnPaginated(int quntityToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quntityToGenerate);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quntityToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (Category outputItem in output.Items)
        {
            var exempleItem = exampleCategoriesList.Find(category => category.Id == outputItem.Id);

            exempleItem.Should().NotBeNull();
            exempleItem.Name.Should().Be(outputItem.Name);
            exempleItem.Description.Should().Be(outputItem.Description);
            exempleItem.IsActive.Should().Be(outputItem.IsActive);
            exempleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    [InlineData("Sci-fi", 1, 2, 2, 5)]
    [InlineData("Sci-fi", 2, 3, 2, 5)]
    [InlineData("Horror", 3, 5, 0, 2)]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Other", 1, 5, 0, 0)]
    [InlineData("Based", 1, 5, 2, 2)]
    public async Task SearchByText(string search, int page, int perPage, int expectedQuantityReturned, int expectQuantityTotalItems)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithName(new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi",
            "Sci-fi - IA",
            "Sci-fi - Robots",
            "Sci-fi - Space",
            "Sci-fi - Future",
            "Fantasia - Based on books"
        });
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityReturned);

        foreach (Category outputItem in output.Items)
        {
            var exempleItem = exampleCategoriesList.Find(category => category.Id == outputItem.Id);

            exempleItem.Should().NotBeNull();
            exempleItem.Name.Should().Be(outputItem.Name);
            exempleItem.Description.Should().Be(outputItem.Description);
            exempleItem.IsActive.Should().Be(outputItem.IsActive);
            exempleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByOrder))]
    [Trait("Integration/Infra.Data", "Category Repository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("id", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "desc")]
    public async Task SearchByOrder(string orderBy, string order)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        var searchOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneOrdered(exampleCategoriesList, orderBy, searchOrder);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);

        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var outputItem = output.Items[i];

            outputItem.Should().NotBeNull();
            expectedItem.Should().NotBeNull();

            expectedItem.Id.Should().Be(outputItem.Id);
            expectedItem.Name.Should().Be(outputItem.Name);
            expectedItem.Description.Should().Be(outputItem.Description);
            expectedItem.IsActive.Should().Be(outputItem.IsActive);
            expectedItem.CreatedAt.Should().Be(outputItem.CreatedAt);
        }
    }
}
