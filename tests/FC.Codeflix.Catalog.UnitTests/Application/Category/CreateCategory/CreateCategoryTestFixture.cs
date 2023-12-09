using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;
[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
{}

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
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
