using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPI.Data;

public class ToDoDbContext: DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options): base(options)
    {
    }

    public DbSet<ToDo> ToDos => Set<ToDo>();
}
