using TestWebAPI.Models;

namespace TestWebAPI.Services;

public interface IEmployeeService
{
    Task SaveEmployeeAsync(Employee employee);
    Task<IEnumerable<Employee>> GetEmployeesAsync();
}