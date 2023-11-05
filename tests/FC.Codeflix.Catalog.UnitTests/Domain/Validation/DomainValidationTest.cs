using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest : BaseFixture
{
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrow))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrow()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        string? value = null;
        Action action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyWhenEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        string target = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should be at leats {minLength} characters long");
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenLess(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MaxLength(target, minLength, fieldName);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberofTests = 5) 
    {
        var faker = new Faker();
        for (int i = 0; i < numberofTests; i++)
        {
            string example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);

            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberofTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberofTests; i++)
        {
            string example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 20);

            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberofTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberofTests; i++)
        {
            string example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 20);

            yield return new object[] { example, maxLength };
        }
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMax(int numberofTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberofTests; i++)
        {
            string example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(1, 20);

            yield return new object[] { example, maxLength };
        }
    }
    }
