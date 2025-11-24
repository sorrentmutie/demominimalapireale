using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoMinimalAPI.HealthChecks;

public class MyCustomHealthCheck : IHealthCheck
{
    private readonly IConfiguration configuration;

    public MyCustomHealthCheck(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        var isHealthy = await PerformSomeCheck(cancellationToken);

        if (isHealthy) {
            return HealthCheckResult.Healthy(
                "Service is operational",
                new Dictionary<string, object>
                {
                    { "check_time", DateTime.UtcNow },
                    { "custom_data", "value"}

                });
        } else
        {
            return HealthCheckResult.Degraded(
                "Service is working but with some issues");
        }
    }

    private async Task<bool> PerformSomeCheck(
        CancellationToken cancellationToken)     
    {
        await Task.Delay(1000, cancellationToken);
        return true;
    }

}
