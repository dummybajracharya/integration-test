using Microsoft.EntityFrameworkCore;
using TestWebAPI.DbContexts;
using TestWebAPI.Models;

namespace TestWebAPI.Services;

public class EmployeeService(EmployeeDbContext context) : IEmployeeService
{
    public async Task SaveEmployeeAsync(Employee employee)
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        var result = await context.Employees.ToListAsync();
        return result;
    }
}