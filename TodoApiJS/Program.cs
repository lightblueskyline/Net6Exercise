using Microsoft.EntityFrameworkCore;
using TodoApiJS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region  Register the database context
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseDefaultFiles(); // enable default file mapping
app.UseStaticFiles(); // server static files

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
