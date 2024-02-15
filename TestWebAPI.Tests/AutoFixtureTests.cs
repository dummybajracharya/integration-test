using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using TestWebAPI.Tests.Models;
using Xunit.Abstractions;

namespace TestWebAPI.Tests;

public class AutoFixtureTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AutoFixtureTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Fixture_With_Freeze_Should_FreezeAndGenerateSameValue()
    {
        var fixture = new Fixture();
        fixture.Freeze<int>(); // When we use Freeze then it will freeze and whenever you create Int, it will generate same value of Int
        fixture.Freeze<string>();
        
        var a = fixture.Create<int>();
        var b = fixture.Create<int>();
        _testOutputHelper.WriteLine(a.ToString());
        _testOutputHelper.WriteLine(b.ToString());

        var c = fixture.Create<string>();
        var d = fixture.Create<string>();
        _testOutputHelper.WriteLine(c);
        _testOutputHelper.WriteLine(d);
    }

    [Fact]
    public void Fixture_Using_With_Build()
    {
        var sut = new Fixture()
            .Build<Student>()
            .With(s => s.Name, "Ariana")
           
            .Create();

        _testOutputHelper.WriteLine(sut.Name);
        _testOutputHelper.WriteLine(sut.Address);
        _testOutputHelper.WriteLine(sut.DateOfBirth.ToString("dd/MM/yyyy"));
        _testOutputHelper.WriteLine(sut.Id.ToString());

    }
    
    [Fact]
    public void Fixture_Using_WithOut_Build()
    {
        var sut = new Fixture()
            .Build<Student>()
            .Without(s => s.Id) // Set Id = 0
            .Without(s => s.DateOfBirth) // Set DateOfBirth = 01/01/0001 or empty value
            .Create();

        _testOutputHelper.WriteLine(sut.Name);
        _testOutputHelper.WriteLine(sut.Address);
        _testOutputHelper.WriteLine(sut.DateOfBirth.ToString("dd/MM/yyyy"));
        _testOutputHelper.WriteLine(sut.Id.ToString());
    }

    [Theory, AutoData] // AutoData requires AutoFixture.Xunit2 nuget package
    public void Fixture_AutoData_GenerateDummyData(string name, DateTime dataOfBirth)
    {
        var student = new Student
        {
            Name = name,
            DateOfBirth = dataOfBirth
        };


        _testOutputHelper.WriteLine(student.Name);
       _testOutputHelper.WriteLine(student.DateOfBirth.ToString("d"));

    }


    [Theory, InlineAutoData]
    [InlineAutoData("", "Kogarah")]
    [InlineAutoData("Ariana")]
    public void Fixture_InlineAutoData_GenerateDummyData(string name, string address)
    {
        var student = new Student();
        student.Name = name;
        student.Address = address;
        
        
        Assert.Equal(name, student.Name);
        Assert.Equal(address, student.Address);
    }


    [Theory, AutoData]
    public void Sample_Integration_Test(Student student)
    {
        var fixture = new Fixture();
        var studentDbContext = Substitute.For<IStudentDbContext>();
        fixture.Inject<IStudentDbContext>(studentDbContext);

        var logger = Substitute.For<ILogger<StudentRepository>>();
        fixture.Inject(logger);
            

        var sut = fixture.Create<StudentRepository>();
        sut.SaveStudent(student);

        studentDbContext.Received(1).Save(student);
        logger.Received().LogInformation("Save");
    }
    
    
    [Fact]
    public void Sample_Integration_Test1()
    {
        //
        // var studentDbContext = Substitute.For<IStudentDbContext>();
        //
        //
        // fixture.Inject<IStudentDbContext>(studentDbContext);
        //
        // var sut = fixture.Create<StudentRepository>();
        // sut.SaveStudent(student);
        //
        // studentDbContext.Received(3).Save(student);
        // studentDbContext.ReceivedCalls();
       
    }
    
    [Fact]
    public void SaveStudent_ShouldBe_Success()
    {
        var student = new Student()
        {
            Id = 1, Name = "Sanjay"
        };
        
        // Create fixture and inject depedency
        var fixture = new Fixture();
        fixture.Inject<IStudentDbContext>(new StudentDbContext());
        
        // create sut 
        var sut = fixture.Create<StudentRepository>();
        sut.SaveStudent(student);
    }
    
    [Fact]
    public void SaveStudent_UsingFreeze_ShouldBe_Success()
    {
        try
        {
            var student = new Student()
            {
                Id = 1, Name = "Sanjay"
            };
        
            // Create fixture and inject depedency
            var fixture = new Fixture();
            //fixture.Customize(new StudentDbContext()).Create();
            
            
            var t = fixture.Create<IStudentDbContext>();
            
            var mockStudentDbContext = Substitute.For<IStudentDbContext>();

            //var mockStudentDbContext1 = fixture.Freeze<IStudentDbContext>(mockStudentDbContext);
        
            //fixture.Inject<IStudentDbContext>(new StudentDbContext());
        
            // Act
            var sut = fixture.Create<StudentRepository>();
            sut.SaveStudent(student);
        
            // Assert
            mockStudentDbContext.Received().Save(student);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        
    }
}