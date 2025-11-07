using DemoMinimalAPI.Data;using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPI
{
    public class ToDoService : IToDoData
    {
        private readonly ToDoDbContext context;

        public ToDoService(ToDoDbContext context)
        {
            this.context = context;
        }

        public async Task<List<ToDo>?> GetAllAsync()
        {
            var todos = await context.ToDos.ToListAsync();
            return todos;
        }

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            var todo = await context.ToDos.FindAsync(id);
            return todo;
        }
    }
}
