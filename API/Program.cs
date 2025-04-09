// API/Program.cs
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using ISITECH__EventsArea.Infrastructure.Data;
using ISITECH__EventsArea.Infrastructure.Repositories;
using ISITECH__EventsArea.Application.Services; // Assurez-vous que le namespace est correct
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AutoMapper;
using ISITECH__EventsArea.API.Mapping;

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<EventsAreasDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Ou spécifiez l'assembly contenant vos profils

// Repositories et UnitOfWork - AJOUTEZ CETTE PARTIE
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Si nécessaire, ajoutez également vos repositories spécifiques ici
// builder.Services.AddScoped<IEventRepository, EventRepository>();
// builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
// etc.

// Services
builder.Services.AddScoped<IEventService, EventService>();

// Controllers
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

// Routes
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();