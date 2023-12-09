using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.UnitTests.Common;
using Xunit;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using Moq;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> 
{ }

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    }

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputLongName()
    {
        var invalidInputLongName = GetValidInput();
        var tooLongNameCategory = "";

        while (tooLongNameCategory.Length <= 255) tooLongNameCategory = tooLongNameCategory + Faker.Commerce.ProductName;
        invalidInputLongName.Name = tooLongNameCategory;

        return invalidInputLongName;
    }

    public UpdateCategoryInput GetInvalidInputDescriptionTooLong()
    {
        var invalidInputLongDescription = GetValidInput();
        var tooLongDescriptionCategory = "";

        while (tooLongDescriptionCategory.Length <= 10000) tooLongDescriptionCategory = tooLongDescriptionCategory + Faker.Commerce.ProductName;
        invalidInputLongDescription.Description = tooLongDescriptionCategory;

        return invalidInputLongDescription;
    }
}
