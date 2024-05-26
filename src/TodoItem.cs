using System.Reflection.Metadata.Ecma335;

namespace dotnet9_openapi_scalar;

public class TodoItem
{
    public TodoItem(Guid id, string title, bool isCompleted)
    {
        Id = id;
        Title = title;  
        IsCompleted = isCompleted;  
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }

    private TodoItem(string title)
    {
        Id = Guid.NewGuid();
        IsCompleted = false;
        Title = title;
    }

    public static TodoItem Add(string  title) => new TodoItem(title);        

    public void Update(string title, bool isCompleted)
    {
        Title = title;
        IsCompleted = isCompleted;
    }
}
