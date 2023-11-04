using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instatiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instatiate()
    {
        var validData = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.IsActive.Should().BeTrue();

        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();   
    }

    [Theory(DisplayName = nameof(Instatiate))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstatiateWithIsActive(bool isActive)
    {
        // Arragen
        var validData = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.IsActive.Should().Be(isActive);

        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstatiateErrorWhenNameIsEmpty(string? name)
    {
        var validData = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(name!, validData.Description);
        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsNull()
    {
        var validData = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(validData.Name, null!);
        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsLessthan3Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void InstatiateErrorWhenNameIsLessthan3Characthers(string? invalidName)
    {
        var validData = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(invalidName!, validData.Description);
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenNameIsGreaterThan255Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenNameIsGreaterThan255Characthers()
    {
        var validData = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        Action action = () => new DomainEntity.Category(invalidName!, validData.Description);

        var exception = Assert.Throws<EntityValidationException>(action);
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsGreaterThan10000Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsGreaterThan10000Characthers()
    {
        var invalidDescription = "";

        while (invalidDescription.Length < 10000) invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        var validData = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(validData.Name, invalidDescription);
        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");
    }

    [Fact(DisplayName = nameof(DeactiveCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactiveCategory()
    {
        var validData = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);

        category.Deactivate();
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        var category = _categoryTestFixture.GetValidCategory();

        category.Activate();
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var newValues = _categoryTestFixture.GetValidCategory();

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyNameCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyNameCategory()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newValues = _categoryTestFixture.GetValidCategory();

        category.Update(newValues.Name);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessthan3Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void UpdateErrorWhenNameIsLessthan3Characthers(string? invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        Action action = () => category.Update(invalidName!, category.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characthers()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        Action action = () => category.Update(invalidName!, category.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10000Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10000Characthers()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = "";

        while (invalidDescription.Length < 10000) invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        Action action = () => category.Update(category.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
        }
    }
}
