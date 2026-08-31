var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var todos = new List<Todo>();
var nextId = 1;

app.MapGet("/hello_ulriken", () => Results.Ok("Hello, Ulriken!"));

app.MapGet("/todos", () => Results.Ok(todos));

app.MapGet("/todos/{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(item => item.Id == id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.MapPost("/todos", (CreateTodo request) =>
{
    var todo = new Todo(nextId++, request.Title, false);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id:int}", (int id, UpdateTodo request) =>
{
    var index = todos.FindIndex(item => item.Id == id);
    if (index < 0)
    {
        return Results.NotFound();
    }

    var todo = new Todo(id, request.Title, request.IsComplete);
    todos[index] = todo;
    return Results.Ok(todo);
});

app.MapDelete("/todos/{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(item => item.Id == id);
    if (todo is null)
    {
        return Results.NotFound();
    }

    todos.Remove(todo);
    return Results.NoContent();
});

app.Run();

record Todo(int Id, string Title, bool IsComplete);
record CreateTodo(string Title);
record UpdateTodo(string Title, bool IsComplete);