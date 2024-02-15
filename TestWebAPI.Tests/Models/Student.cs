using Microsoft.Extensions.Logging;

namespace TestWebAPI.Tests.Models;

public class Student
{
    public string Name { get; set; }
    public int Id { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string Address { get; set; }
}

public class StudentRepository(IStudentDbContext context, ILogger<StudentRepository> logger)
{
    public string Name => "Student Repo";

    public void SaveStudent(Student student)
    {
        context.Save(student);
        logger.LogInformation("Save");
    }
}


public class StudentDbContext : IStudentDbContext
{
    public string Name => "StudentDbContext Ho";
    public string Identity => Guid.NewGuid().ToString();
    
    public void Save(Student student)
    {
        Console.WriteLine("Save student");
    }
}

public interface IStudentDbContext
{
    string Identity { get; }
    void Save(Student student);
}