namespace DemoMinimalAPI.Interfaces;

public interface IToDoData
{
    Task<List<ToDo>?> GetAllAsync();
    Task<ToDo?> GetByIdAsync(int id);
}
