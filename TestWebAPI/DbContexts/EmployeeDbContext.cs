using Microsoft.EntityFrameworkCore;
using TestWebAPI.Models;

namespace TestWebAPI.DbContexts;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions options) 
        : base(options)
    {
        
    }

    public DbSet<Employee> Employees { get; set; }
}