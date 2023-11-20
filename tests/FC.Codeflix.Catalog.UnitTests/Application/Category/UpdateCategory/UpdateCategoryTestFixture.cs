using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.UnitTests.Common;
using Xunit;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using Moq;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> 
{ }

public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidCategoryName()
    {
        string categoryName = "";

        while (categoryName.Length < 3) categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255) categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        string categoryDescription = "";

        if (categoryDescription.Length > 10000) categoryDescription = categoryDescription[..10000];
        return categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntity.Category GetValidCategory() => new(GetValidCategoryName(), Faker.Commerce.ProductDescription(), GetRandomBoolean());

    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    }
}
