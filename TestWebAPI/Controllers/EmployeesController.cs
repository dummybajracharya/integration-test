using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Models;
using TestWebAPI.Services;

namespace TestWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController(
    ILogger<EmployeesController> logger,
    IEmployeeService service)
    : ControllerBase
{
    
    [HttpGet("GetEmployees")]
    public async Task<IEnumerable<Employee>> Get()
    {
        try
        {
            var result = await service.GetEmployeesAsync();
            logger.LogInformation("Success");
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    [HttpPost("SaveEmployee")]
    public async Task SaveEmployee(Employee employee)
    {
        try
        {
            await service.SaveEmployeeAsync(employee);
            logger.LogInformation("Save successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save employee");
            throw;
        }
    }
}