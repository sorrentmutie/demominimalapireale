using DemoMinimalAPI.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoMinimalAPI.Extensions;

public static class HealthCheckExtension
{
    public static void RegisterHealthCheck(this IServiceCollection services,
        IConfiguration configuration)
    {

        string NorthwindConnectionString =
            configuration.GetConnectionString("Northwind") ?? "";

        services.AddHealthChecks().AddSqlServer(
            NorthwindConnectionString,
            healthQuery: "SELECT 1;",
            failureStatus: HealthStatus.Degraded,
            tags: new[] { "db", "sql", "sqlserver" },
            name: "sql-server",
            timeout: TimeSpan.FromSeconds(5));

        services.AddHealthChecks()
            .AddUrlGroup(
              new Uri("https://randomuser.me/api"),
              name: "external-api",
              tags: new[] { "external" },
              failureStatus: HealthStatus.Degraded);

        services.AddHealthChecks()
            .AddCheck<MyCustomHealthCheck>(
               "custom-check",
               tags: new[] { "custom" }
             );

        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(10);
            setup.MaximumHistoryEntriesPerEndpoint(50);
        })
        .AddInMemoryStorage();
    }
}
