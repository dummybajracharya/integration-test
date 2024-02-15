using System.Collections;
using Bogus;
using TestWebAPI.Models;

namespace TestWebAPI.Tests;

public static class DataFixture
{
    public static List<Employee> GetEmployee(int count, bool useNewSeed = false)
    {
        return GetEmployeeFaker(useNewSeed).Generate(count);
    }

    private static Faker<Employee> GetEmployeeFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        return new Faker<Employee>()
            .RuleFor(e => e.Id, o => 0)
            .RuleFor(e => e.Name, (faker, t) => faker.Name.FirstName())
            .RuleFor(e => e.Path, (faker, t) => faker.Address.City())
            .UseSeed(seed);
    }
}