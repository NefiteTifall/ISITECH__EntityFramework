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
using ISITECH__EventsArea.Infrastructure.Data.Seeders;

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
builder.Services.AddControllers(options => options.Filters.Add<ModelStateValidationFilter>())
	.AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


// Routes
builder.Services.AddRouting(options => { options.LowercaseUrls = true; });


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

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

if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		var context = services.GetRequiredService<EventsAreasDbContext>();

		try
		{
			await EventsAreaSeeder.SeedAsync(context);
		}
		catch (Exception ex)
		{
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "Une erreur s'est produite lors de l'initialisation de la base de données.");
		}
	}
}

app.Run();

app.Run();