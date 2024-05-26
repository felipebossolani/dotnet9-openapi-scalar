using dotnet9_openapi_scalar;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/todos", () => TodoStore.GetAll());

app.MapGet("/todos/{id}", (Guid id) =>
{
    var todo = TodoStore.Get(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/todos", (TodoInsertRequest request) =>
{
    var todo = TodoItem.Add(request.Title);
    TodoStore.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", (Guid id, TodoUpdateRequest request) =>
{
    var todo = TodoStore.Get(id);
    if (todo is null)    
        return Results.NotFound();

    todo.Update(request.Title, request.IsComplete);
    TodoStore.Update(todo);
    return Results.Ok(todo); 
});

app.MapDelete("/todos/{id}", (Guid id) =>
{
    if (TodoStore.Get(id) is null)
        return Results.NotFound();
    TodoStore.Delete(id);
    return Results.Ok();
});

app.Run();

public partial class Program { }

public record TodoInsertRequest(string Title);
public record TodoUpdateRequest(string Title, bool IsComplete);

