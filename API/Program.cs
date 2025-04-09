using ISITECH__EventsArea;
using ISITECH__EventsArea.Infrastructure.Data;
using ISITECH__EventsArea.Domain.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// (1) Ajouter EF Core au conteneur de services
// On lit la chaîne de connexion "DefaultConnection" depuis appsettings.json
builder.Services.AddDbContext<EventsAreasDbContext>(options =>
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

app.Run();