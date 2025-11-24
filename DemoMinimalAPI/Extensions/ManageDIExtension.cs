namespace DemoMinimalAPI.Extensions;

public static class ManageDIExtension
{
    public static void RegisterAllServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFeatureManagement();

        services.AddApplicationInsightsTelemetry();
        services.RegisterHealthCheck(configuration);
        services.RegistersServices(configuration);
        services.RegisterNorthwindDatabase(configuration);

    }
}
