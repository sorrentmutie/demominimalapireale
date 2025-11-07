namespace DemoMinimalAPI.Interfaces;

public interface IToDoData
{
    Task<List<ToDo>?> GetAllAsync();
    Task<ToDo?> GetByIdAsync(int id);
    Task<ToDo> CreateAsync(string title);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, ToDo toDoChanged);
    Task<bool> PatchAsync(int id, ToDo toDoChanged);
}
