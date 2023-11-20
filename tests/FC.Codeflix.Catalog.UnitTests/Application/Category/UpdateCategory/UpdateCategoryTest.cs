using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Moq;
using Xunit;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestdataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestdataGenerator))]
    public async void UpdateCategory(DomainEntity.Category exampleCategory, UseCases.UpdateCategoryInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        repositoryMock.Verify(repository => repository.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory = Use Cases")]
    public async void ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var exampleGuid = Guid.NewGuid();
        var input = _fixture.GetValidInput(exampleGuid);

        repositoryMock.Setup(x => x.Get(exampleGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));

        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository => repository.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestdataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestdataGenerator))]
    public async void UpdateCategoryWithoutProvidingIsActive(DomainEntity.Category exampleCategory, UseCases.UpdateCategoryInput exampleInput)
    {
        var input = new UseCases.UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);

        repositoryMock.Verify(repository => repository.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryProvidingOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestdataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestdataGenerator))]
    public async void UpdateCategoryProvidingOnlyName(DomainEntity.Category exampleCategory, UseCases.UpdateCategoryInput exampleInput)
    {
        var input = new UseCases.UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);

        repositoryMock.Verify(repository => repository.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
