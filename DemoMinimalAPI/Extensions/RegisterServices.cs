using DemoMinimalAPI.Configurations;
using DemoMinimalAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPI.Extensions;

public static class RegisterServices
{
    public static void RegistersServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Here you can register your services, for example:
        // services.AddScoped<IToDoData, ToDoDataService>();

        var connectionString = configuration
            .GetConnectionString("ToDoDatabase") ?? "DefaultName";  

        services.AddDbContext<ToDoDbContext>
         (options => options.UseInMemoryDatabase(connectionString));

        services.AddScoped<IToDoData, ToDoService>();
        services.AddOptions<MyApp>()
            .Bind(configuration.GetSection("MyApp"))
            .ValidateDataAnnotations();

    }
}
