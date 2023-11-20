using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Moq;
using Xunit;
using FC.Codeflix.Catalog.Application.Exceptions;
using FluentAssertions;


namespace FC.Codeflix.Catalog.UnitTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory = Use Cases")]
    public async void DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var categoryExample = _fixture.GetValidCategory();
        var input = new UseCase.DeleteCategoryInput(categoryExample.Id);

        repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);


    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory = Use Cases")]
    public async void ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var exampleGuid = Guid.NewGuid();
        var input = new UseCase.DeleteCategoryInput(exampleGuid);

        repositoryMock.Setup(x => x.Get(exampleGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));

        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository => repository.Get(exampleGuid, It.IsAny<CancellationToken>()), Times.Once);
    }
}
