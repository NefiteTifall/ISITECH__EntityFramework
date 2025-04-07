using IsitechEfCoreApp;
using IsitechEfCoreApp.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// (1) Ajouter EF Core au conteneur de services
// On lit la chaîne de connexion "DefaultConnection" depuis appsettings.json
builder.Services.AddDbContext<MyDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapGet("/todos", async (IsitechEfCoreApp.MyDbContext db) => await db.Todos.ToListAsync());
app.MapGet("/todos/{id}", async (int id, IsitechEfCoreApp.MyDbContext db) => await db.Todos.FindAsync(id) is { } todo ? Results.Ok((object?)todo) : Results.NotFound());
app.MapPost("/todos", async (TodoItem todo, IsitechEfCoreApp.MyDbContext db) =>
{
  db.Todos.Add(todo);
  await db.SaveChangesAsync();
  return Results.Created($"/todos/{todo.Id}", todo);
});
app.MapPut("/todos/{id}", async (int id, TodoItem updatedTodo, IsitechEfCoreApp.MyDbContext db) =>
{
  var todo = await db.Todos.FindAsync(id);
  if (todo is null) return Results.NotFound();

  todo.Title = updatedTodo.Title;
  todo.IsComplete = updatedTodo.IsComplete;
  await db.SaveChangesAsync();
  
  return Results.NoContent();
});
app.MapDelete("/todos/{id}", async (int id, IsitechEfCoreApp.MyDbContext db) =>
{
  var todo = await db.Todos.FindAsync(id);
  if (todo is null) return Results.NotFound();

  db.Todos.Remove(todo);
  await db.SaveChangesAsync();
  
  return Results.NoContent();
});

app.Run();