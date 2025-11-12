using DemoMinimalAPI.Data;
using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemoMinimalAPI.Extensions;

public static class NorthwindDatabaseService
{
    public static void RegisterNorthwindDatabase(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
         .GetConnectionString("Northwind") ?? "DefaultName";

        services.AddDbContext<NorthwindContext>
         (options => options.UseSqlServer(connectionString));

    }
}
