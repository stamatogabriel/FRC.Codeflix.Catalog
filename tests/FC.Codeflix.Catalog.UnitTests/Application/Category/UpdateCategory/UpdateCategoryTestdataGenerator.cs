﻿using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

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

    public static IEnumerable<object[]> GetInvalidInputs(int times = 15)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

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
