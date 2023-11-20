using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;
public class UpdateCategoryTestdataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();

        for (int i = 0; i < times; i++)
        {
            var exampleCategory = fixture.GetValidCategory();
            var exampleInput = fixture.GetValidInput(exampleCategory.Id);
            yield return new object[] { exampleCategory, exampleInput };
        }
    }
}
