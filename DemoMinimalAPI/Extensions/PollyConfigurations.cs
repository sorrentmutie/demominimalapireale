using Polly.Extensions.Http;

namespace DemoMinimalAPI.Extensions;

public static class PollyConfigurations
{

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3,
            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }

    public static void ConfigurePolly(this IServiceCollection services)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3,
            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 2,
            durationOfBreak: TimeSpan.FromSeconds(10));

        services.AddHttpClient("SampleClient")
                .AddPolicyHandler(GetRetryPolicy());


        services.AddHttpClient("ExternalApiClient", client =>
                {
                    client.BaseAddress = new Uri("https://api.xxxchucknorris.io/");
                })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(circuitBreakerPolicy);
    }
}
