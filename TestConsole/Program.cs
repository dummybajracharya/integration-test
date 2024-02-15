// See https://aka.ms/new-console-template for more information

using Bogus;
//
// Console.WriteLine("Hello, World!");
//
// var result = DataFixture.GetEmployee(3);
//
// foreach (var employee in result)
// {
//     Console.WriteLine("Name");
//     Console.WriteLine(employee.Name);
//     Console.WriteLine("Path");
//     Console.WriteLine(employee.Path);
// }
//
// Console.WriteLine(result);

var result = new Faker<Employee>()
    .RuleFor(e => e.Id, o => 0)
    .RuleFor(e => e.Name, (faker, t) => faker.Name.FirstName())
    .RuleFor(e => e.Path, (faker, t) => faker.Address.City())
    .UseSeed(20).Generate(2);

foreach (var employee in result)
{
    Console.WriteLine($"Id - {employee.Id}");
    Console.WriteLine($"Name - {employee.Name}");
    Console.WriteLine($"Path - {employee.Path}");
}








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


public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}