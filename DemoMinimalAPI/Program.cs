using DemoMinimalAPI.Configurations;
using DemoMinimalAPI.Data;
using DemoMinimalAPI.Extensions;
using DemoMinimalAPI.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegistersServices(builder.Configuration);
builder.Services.RegisterNorthwindDatabase(builder.Configuration);


var app = builder.Build();

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

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapToDoEndpoints();
app.MapNorthwindCategoriesEndpoints();

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

app.UseHttpsRedirection();

app.Run();


