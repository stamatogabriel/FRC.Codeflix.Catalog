using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.ListCategories;
public class ListCategoriesTestDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameters(int times = 15)
    {
        var fixture = new ListCategoryTestFixture();
        var inputExemple = fixture.GetExampleInput();

        for (int i = 0; i < times; i++)
        {
            switch (i % 5) 
            {
                case 0:
                    yield return new object[] { new ListCategoriesInput() };
                    break;

                case 1:
                    yield return new object[] { new ListCategoriesInput(inputExemple.Page) };
                    break;

                case 2:
                    yield return new object[] { new ListCategoriesInput(inputExemple.Page, inputExemple.PerPage) };
                    break;

                case 3:
                    yield return new object[] { new ListCategoriesInput(inputExemple.Page, inputExemple.PerPage, inputExemple.Search) };
                    break;

                case 4:
                    yield return new object[] { new ListCategoriesInput(inputExemple.Page, inputExemple.PerPage, inputExemple.Search, inputExemple.Sort) };
                    break;

                case 5:
                    yield return new object[] { inputExemple };
                    break;

                default:
                    yield return new object[] { new ListCategoriesInput() };
                    break;

            }
        }
    }
}
