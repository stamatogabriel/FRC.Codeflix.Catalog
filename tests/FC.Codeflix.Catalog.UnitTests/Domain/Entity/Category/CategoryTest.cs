using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;


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
}
