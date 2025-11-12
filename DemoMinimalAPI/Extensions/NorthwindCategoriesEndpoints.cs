using DemoMinimalAPI.DTO;
using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPI.Extensions;

public static class NorthwindCategoriesEndpoints
{
    public static RouteGroupBuilder MapNorthwindCategoriesEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/categories").WithTags("Categories");

        group.MapGet("/", async (NorthwindContext context) => {
            var categories = await context
            .Categories.Include(c => c.Products).ToListAsync();

            var dtos = categories.Select(c => new CategoryDTO
            {
                 Id = c.CategoryId,
                 Name = c.CategoryName,
                 Description = c.Description,
                 NumberOfProducts = c.Products.Count
            });


            return Results.Ok(dtos);
        });

        group.MapGet("/{id}", async (NorthwindContext context, int id) =>
        {
            var category = await context
                .Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return Results.NotFound();
            } else
            {
                return Results.Ok(new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                    Description = category.Description,
                    NumberOfProducts = category.Products.Count
                });
            }

        });


        group.MapPost("/", async (NorthwindContext context,
            CategoryCreateDTO newCategory) =>
        {
            var category = new Category
            {
                CategoryName = newCategory.Name,
                Description = newCategory.Description
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return Results.Created($"/categories/{category.CategoryId}", 
                new CategoryDTO
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                    Description = category.Description,
                    NumberOfProducts = 0
                });
        });


        return group; 
    }
}
