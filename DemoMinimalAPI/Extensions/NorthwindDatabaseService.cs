using DemoMinimalAPI.Data;
using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemoMinimalAPI.Extensions;

public static class NorthwindDatabaseService
{
    public static void AddDataInMemory(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbcontext =
                  scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            dbcontext.ToDos.AddRange(
                  new ToDo(1, "Learn Minimal APIs", true),
                  new ToDo(2, "Build a Minimal API", false),
                  new ToDo(3, "Profit!", false)
            );
            dbcontext.SaveChanges();
        }
    }


    public static void RegisterNorthwindDatabase(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
         .GetConnectionString("Northwind") ?? "DefaultName";

        services.AddDbContext<NorthwindContext>
         (options => options.UseSqlServer(connectionString));

    }
}
