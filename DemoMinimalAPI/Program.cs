using DemoMinimalAPI.Data;
using DemoMinimalAPI.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IToDoData, ToDoService>();
builder.Services.AddDbContext<ToDoDbContext>
    (options => options.UseInMemoryDatabase("ToDoDb"));

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

app.UseHttpsRedirection();

app.Run();


