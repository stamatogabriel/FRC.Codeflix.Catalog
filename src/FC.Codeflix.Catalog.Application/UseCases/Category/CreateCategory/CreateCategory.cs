using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancelationToken)
    {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

        await _categoryRepository.Insert(category, cancelationToken);
        await _unitOfWork.Commit(cancelationToken);

        return new CreateCategoryOutput(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
    }
}
