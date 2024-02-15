using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Testcontainers.MsSql;
using TestWebAPI.Controllers;
using TestWebAPI.DbContexts;
using TestWebAPI.Tests;

namespace TestcontainersModules;

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<TestWebAPI.Program>, IAsyncLifetime
{
    // Create container. We can override password also
    private readonly MsSqlContainer _msSqlContainer
        = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
       
        // Todo: Use this for integration to replace or override existing service
        builder.ConfigureTestServices(services =>
        {
            // Note: Find services that are registered and removed registration
            // Remove the existing service with the test one
            // var descriptor = services.SingleOrDefault(i => i.ServiceType ==  typeof(ILogger<EmployeesController>));
            // if (descriptor is not null)
            // {
            //     services.Remove(descriptor);
            // }
            
            // Working Code
            // var mockLogger = MockLoggerFactory.Create<EmployeesController>();
            // services.AddSingleton(typeof(ILogger<EmployeesController>), mockLogger);
            
            // Test with NSubsittite
            var mockLogger = Substitute.For<ILogger<EmployeesController>>();
            services.AddSingleton(typeof(ILogger<EmployeesController>), mockLogger);
        });
        
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EmployeeDbContext>));
            services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());
            });
        });
    }
    
    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var scopeService = scope.ServiceProvider;
        var dbContext = scopeService.GetRequiredService<EmployeeDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        // Create some sample data: Replace with Bogus data (Nuget package that generate dummy data)
        // var employee1 = new Employee
        // {
        //     Name = "Sanz",
        //     Path = "sample path"
        // };
        //
        // var employee2 = new Employee
        // {
        //     Name = "Ariana",
        //     Path = "ariana path"
        // };
        //
        // await dbContext.Employees.AddRangeAsync(new List<Employee>
        // {
        //     employee1, employee2
        // });
        
        // Above code can be replace using Bogus
        await dbContext.Employees.AddRangeAsync(DataFixture.GetEmployee(5));
        await dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}