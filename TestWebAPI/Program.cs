using Microsoft.EntityFrameworkCore;
using TestWebAPI.DbContexts;
using TestWebAPI.Services;

namespace TestWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseSqlServer(
                        "server=.\\SQL2017;database=Sanjay;user id=sa;password=P@ssword!1;TrustServerCertificate=true"));

                builder.Services.AddScoped<IEmployeeService, EmployeeService>();
                var app = builder.Build();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
           
        }
    }
}