using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;

#region Name
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList")); // 依賴注入數據庫上下文
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // 顯示數據庫異常

#region Config JSON options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.IncludeFields = true;
});
#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region JsonOptions
app.MapGet("/JsonOptions", () =>
    new TodoJson { Name = "JsonOptions", IsComplete = false, });
#endregion

#region JsonSerializerOptions
var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
app.MapGet("/JsonSerializerOptions", () =>
    Results.Json(new TodoJson1 { Name = "JsonSerializerOptions", IsComplete = true, }, options));
#endregion

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.Select(x => new TodoDTO(x)).ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).Select(x => new TodoDTO(x)).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(new TodoDTO(todo)) : Results.NotFound());

app.MapPost("/todoitems", async (TodoDTO todoDTO, TodoDb db) =>
{
    var todoItem = new Todo
    {
        Name = todoDTO.Name,
        IsComplete = todoDTO.IsComplete,
    };

    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", new TodoDTO(todoItem));
});

app.MapPut("/todoitems/{id}", async (int id, TodoDTO inputTodoDTO, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Name = inputTodoDTO.Name;
    todo.IsComplete = inputTodoDTO.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(new TodoDTO(todo));
    }

    return Results.NoContent();
});

app.Run();
#endregion

#region 其他
record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// JsonOptions
class TodoJson
{
    public string? Name;
    public bool IsComplete;
}

class TodoJson1
{
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
}

class TodoDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoDTO() { }
    public TodoDTO(Todo todoItem) =>
        (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
#endregion
