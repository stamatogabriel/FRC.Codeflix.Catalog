using Xunit;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
{}
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{}
