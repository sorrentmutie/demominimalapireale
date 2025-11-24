// Remember to configure Serilog BEFORE building the builder
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File(path: "logs/log-.txt",
       rollingInterval: RollingInterval.Day,
       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();

    // Register all services
    builder.Services.ConfigurePolly();
    builder.Services.RegisterAllServices(builder.Configuration);

    var app = builder.Build();
    // We need to add the middleware for request logging with Serilog

    // Add data to memory with EF Core in memory
    app.AddDataInMemory();
    // Manage all endpoints
    app.ManageAllEndpoints();

    app.Run();

}
catch (Exception ex)
{

    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}




