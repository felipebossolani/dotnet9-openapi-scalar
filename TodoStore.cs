namespace dotnet9_openapi_scalar;

public static class TodoStore
{
    private static readonly List<TodoItem> Todos = new();
    
    public static List<TodoItem> GetAll() => Todos;

    public static TodoItem? Get(Guid id) => Todos.FirstOrDefault(t => t.Id == id);

    public static TodoItem Add(TodoItem todo)
    {
        Todos.Add(todo);
        return todo;
    }

    public static void Update(TodoItem todo)
    {
        var index = Todos.FindIndex(t => t.Id == todo.Id);
        Todos[index] = todo;        
    }

    public static void Delete(Guid id)
    {
        var todo = Get(id);
        if (todo is null) throw new ArgumentNullException(nameof(todo));
        Todos.Remove(todo);
    }
}
