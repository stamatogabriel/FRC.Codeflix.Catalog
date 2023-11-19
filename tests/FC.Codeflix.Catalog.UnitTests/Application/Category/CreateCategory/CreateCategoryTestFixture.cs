using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;
[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : BaseFixture
{
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

    public CreateCategoryInput GetValidInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputLongName()
    {
        var invalidInputLongName = GetValidInput();
        var tooLongNameCategory = "";

        while (tooLongNameCategory.Length <= 255) tooLongNameCategory = tooLongNameCategory + Faker.Commerce.ProductName;
        invalidInputLongName.Name = tooLongNameCategory;

        return invalidInputLongName;
    }

    public CreateCategoryInput GetInvalidInputNameNull()
    {
        var invalidInputNameNull = GetValidInput();
        invalidInputNameNull.Name = null;

        return invalidInputNameNull;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var invalidInputDescriptionNull = GetValidInput();
        invalidInputDescriptionNull.Description = null;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputDescriptionTooLong()
    {
        var invalidInputLongDescription = GetValidInput();
        var tooLongDescriptionCategory = "";

        while (tooLongDescriptionCategory.Length <= 10000) tooLongDescriptionCategory = tooLongDescriptionCategory + Faker.Commerce.ProductName;
        invalidInputLongDescription.Description = tooLongDescriptionCategory;

        return invalidInputLongDescription;
    }
}
