using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace DemoMinimalAPI.Extensions;

public static class FullEndpointsManagement
{
    public static void ManageAllEndpoints(
        this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapToDoEndpoints();
        app.MapProductEndpoints();

        app.MapNorthwindCategoriesEndpoints();

        app.MapHealthChecks("/health/db",
    new HealthCheckOptions { Predicate = check => check.Tags.Contains("db") });

        app.MapHealthChecks("/health/external",
            new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("external"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });


        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });


        //app.UseHealthChecks("/health-ui", new HealthCheckOptions()
        //{
        //    Predicate = _ => true,
        //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //});

        app.MapHealthChecksUI(options => options.UIPath = "/health-ui");


        app.MapGet("/conf", (IConfiguration configuration) => {
            // throw new Exception("some error");
            var message = configuration["MyEnv2"];
            return Results.Ok(message);
        });

        //app.MapGet("/conf", (IOptions<MyApp> options)
        //    =>
        //{
        //    var message = options.Value.Message;
        //    var pageSize = options.Value.PageSize;
        //    var featureX = options.Value.EnableFeatureX;
        //    return message + $" {pageSize} {featureX}";
        //}).WithTags("Configuration");

        //app.MapGet("/api/products/{id:int:min(1)}", (int id) => Results.Ok(new { Id = id }))
        //    .WithTags("Products");

        //app.MapGet("/api/users/{userName}", (string userName) => {
        //    return Results.Ok( new { Username =  userName }); 
        //})
        //    .WithTags("Users");

        //var v1 = app.MapGroup("/api/v1");
        //var v2 = app.MapGroup("/api/v2");

        //v1.MapGet("/categories/{id:int}",
        //    (int id) => Results.Ok(new { Id = id, Version = 1 }))
        //    .WithTags("v1/Categories");

        //v2.MapGet("/categories/{id:int}",
        //    (int id) => Results.Ok(new { Id = id * 3, Version = 2 }))
        //     .WithTags("v2/Categories");


        //app.MapGet("/tags/{name?}", (string? name) => Results.Ok(new {name}))
        //    .AddEndpointFilter(new NotEmptyFilter())
        //    .WithTags("tags");


        app.MapGet("/joke", async (IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient("ExternalApiClient");

            var response = await client.GetAsync("jokes/random");

            if (!response.IsSuccessStatusCode)
                return Results.Problem("Servizio esterno non raggiungibile");

            var content = await response.Content.ReadAsStringAsync();

            return Results.Ok(content);
        });


        app.UseHttpsRedirection();

    }
}
