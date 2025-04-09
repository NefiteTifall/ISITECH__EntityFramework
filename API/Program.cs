using ISITECH__EventsArea;
using ISITECH__EventsArea.Infrastructure.Data;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Services;
using ISITECH__EventsArea.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// (1) Ajouter EF Core au conteneur de services
// On lit la chaîne de connexion "DefaultConnection" depuis appsettings.json
builder.Services.AddDbContext<EventsAreasDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// 
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<IEventService, EventService>();

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