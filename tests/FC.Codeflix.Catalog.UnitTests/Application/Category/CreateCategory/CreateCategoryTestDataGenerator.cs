using FC.Codeflix.Catalog.UnitTests.Application.Category;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;
public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 15)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 5;

        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                       fixture.GetInvalidInputShortName(),
                       "Name should be at leats 3 characters long"
                    });
                    break;

                case 1:
                    invalidInputsList.Add(new object[]
                    {
                       fixture.GetInvalidInputLongName(),
                       "Name should be less or equal 255 characters long"
                    });
                    break;

                case 2:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputNameNull(),
                        "Name should not be empty or null"
                    });
                    break;

                case 3:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputDescriptionNull(),
                        "Description should not be null"
                    });
                    break;

                case 4:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputDescriptionTooLong(),
                        "Description should be less or equal 10000 characters long"
                    });
                    break;


                default: break;
            }
        }

        return invalidInputsList;
    }

}
