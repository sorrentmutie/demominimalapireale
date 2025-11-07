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

        public async Task<ToDo> CreateAsync(string title)
        {
            var  todo = new ToDo(0, title, false);
            context.ToDos.Add(todo);
            await context.SaveChangesAsync();
            return todo with { Id = todo.Id };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await context.ToDos.FindAsync(id);
            if(todo is not null)
            {
                context.ToDos.Remove(todo);
                await context.SaveChangesAsync();
                return true;
            } 
            return false;   
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

        public async Task<bool> PatchAsync(int id, ToDo toDoChanged)
        {
            var todo = await context.ToDos.FindAsync(id);

            if (todo is not null)
            {
                context.Entry(todo).State = EntityState.Detached;

                var title = "";
                if (!string.IsNullOrEmpty(toDoChanged.Title))
                {
                    title = toDoChanged.Title;
                } else
                {
                    title = todo.Title;
                }
                var tempObject = todo with
                {
                    Title = title,
                    IsCompleted = toDoChanged.IsCompleted
                };
                context.Entry(tempObject).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id, ToDo toDoChanged)
        {
            var todo = await context.ToDos.FindAsync(id);
            if (todo is not null)
            {
                context.Entry(todo).State = EntityState.Detached;
                var x = todo with { 
                   Title = toDoChanged.Title,
                   IsCompleted = toDoChanged.IsCompleted
                };
                context.Entry(x).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
