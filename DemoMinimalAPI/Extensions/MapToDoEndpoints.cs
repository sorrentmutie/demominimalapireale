using Microsoft.AspNetCore.Http.HttpResults;

namespace DemoMinimalAPI.Extensions;

public static class ToDoEndpoints
{
    public static RouteGroupBuilder MapToDoEndpoints(
        this IEndpointRouteBuilder app) 
    {
        var group = app.MapGroup("/todos").WithTags("ToDos");

        group.MapGet("/", async (IToDoData toDoService) =>
            {
                var todos = await toDoService.GetAllAsync();
                return TypedResults.Ok(todos);
            })
           .WithName("GetAllToDos");
           
        group.MapGet("/{id}", async
            Task<Results<Ok<ToDo>, NotFound>> (IToDoData service, int id) =>
        {
            var todo = await service.GetByIdAsync(id);
            return todo is not null ? TypedResults.Ok(todo) : TypedResults.NotFound();
        })
        .WithName("GetToDo");

        group.MapPost("/", async Task<Results<Created<ToDo>, BadRequest<string>>> (IToDoData toDoService, string title) =>
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                return TypedResults.BadRequest("Title cannot be empty.");
            }

            var todo = await toDoService.CreateAsync(title);
            return TypedResults.Created($"/todos/{todo.Id}", todo);
        });

        group.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (IToDoData service,  int id ) =>
        {
            var removed = await service.DeleteAsync(id);

            if(removed)
            {
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }

        });


        group.MapPut("/{id:int}", async Task<Results<NoContent, NotFound, BadRequest>> (IToDoData service, int id, ToDo todoChanged) =>
        {
            if(id != todoChanged.Id)
            {
                return TypedResults.BadRequest();
            }

            var changed = await service.UpdateAsync(id, todoChanged);

            if (changed)
            {
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        });

        group.MapPatch("/{id:int}", async Task<Results<NoContent, NotFound>> (IToDoData service, int id, ToDo todoChanged) =>
        {
            var changed = await service.PatchAsync(id, todoChanged);

            if (changed)
            {
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }


        });

        return group;
    }
}
