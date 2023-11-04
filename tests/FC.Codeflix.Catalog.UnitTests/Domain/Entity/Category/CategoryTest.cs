using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;


namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instatiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instatiate()
    {
        // Arragen
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };

        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;
        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(Instatiate))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstatiateWithIsActive(bool isActive)
    {
        // Arragen
        var validData = new
        {
            Name = "category name",
            Description = "category description",
            IsActive = isActive
        };

        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        
        var dateTimeAfter = DateTime.Now;
        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(category.IsActive, isActive);
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstatiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("category name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsLessthan3Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("aa")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("1")]
    public void InstatiateErrorWhenNameIsLessthan3Characthers(string? invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenNameIsGreaterThan255Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenNameIsGreaterThan255Characthers()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Category(invalidName!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsGreaterThan10000Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsGreaterThan10000Characthers()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Category("category name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(DeactiveCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactiveCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
            IsActive = true
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);

        category.Deactivate();
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
            IsActive = false
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);

        category.Activate();
        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name", Description = "New Description"};

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyNameCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyNameCategory()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name" };

        category.Update(newValues.Name);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal("Category Description", category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessthan3Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("aa")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("1")]
    public void UpdateErrorWhenNameIsLessthan3Characthers(string? invalidName)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action = () => category.Update(invalidName!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characthers()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => category.Update(invalidName!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10000Characthers))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10000Characthers()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action = () => category.Update("category name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }
}
