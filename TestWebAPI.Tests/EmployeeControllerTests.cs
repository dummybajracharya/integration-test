using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using TestcontainersModules;
using TestWebAPI.Controllers;
using TestWebAPI.Models;

namespace TestWebAPI.Tests;

public class EmployeeControllerTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EmployeeControllerTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEmployees_ShouldReturn_Employees()
    {
        var response = await _client.GetAsync("Employees/GetEmployees");
        var result = await response.Content.ReadFromJsonAsync<List<Employee>>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        result?.Count.Should().Be(5);
    }


    [Fact]
    public async Task SaveEmployees_Should_AddNewEmployee()
    {
        var newEmployee = new Employee()
        {
            Name = "Aarav",
            Path = "Aarav home ground"
        };

        var jsonString = JsonConvert.SerializeObject(newEmployee);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("Employees/SaveEmployee", content);
        var result = await response.Content.ReadFromJsonAsync<List<Employee>>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        result?.Count.Should().Be(2);
    }


    [Fact]
    public async Task GetEmployees_ShouldReturn_Employees_WithSuccessLogMessage()
    {
        var response = await _client.GetAsync("Employees/GetEmployees");
        var result = await response.Content.ReadFromJsonAsync<List<Employee>>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        result?.Count.Should().Be(5);

        var mockLogger = _factory.Services.GetRequiredService<ILogger<EmployeesController>>();
        mockLogger.Received().LogInformation("Success");
    }
}