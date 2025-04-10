using ISITECH__EventsArea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Infrastructure.Data.Seeders
{
    public static class EventsAreaSeeder
    {
        public static async Task SeedAsync(EventsAreasDbContext context)
        {
            // Assurez-vous que la base de données est créée et que toutes les migrations sont appliquées
            await context.Database.MigrateAsync();
            
            // Appelez toutes les méthodes de seed
            await SeedCategoriesAsync(context);
            await SeedLocationsAsync(context);
            await SeedRoomsAsync(context);
            await SeedEventsAsync(context);
            await SeedParticipantsAsync(context);
            await SeedSpeakersAsync(context);
            
            // Sauvegardez les changements
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedCategoriesAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des catégories existent déjà
            if (await context.EventCategories.AnyAsync())
                return;
            
            // Ajoutez des catégories
            var categories = new[]
            {
                new EventCategory { Name = "Conférence", Description = "Événement avec présentations et discussions" },
                new EventCategory { Name = "Workshop", Description = "Session pratique et interactive" },
                new EventCategory { Name = "Séminaire", Description = "Réunion éducative sur un sujet spécifique" },
                new EventCategory { Name = "Networking", Description = "Événement axé sur le réseautage professionnel" },
                new EventCategory { Name = "Formation", Description = "Session éducative approfondie" }
            };
            
            await context.EventCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedLocationsAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des lieux existent déjà
            if (await context.Locations.AnyAsync())
                return;
            
            // Ajoutez des lieux
            var locations = new[]
            {
                new Location 
                { 
                    Name = "Centre de Conférences Paris", 
                    Address = "123 Avenue des Champs-Élysées", 
                    City = "Paris", 
                    Country = "France", 
                    Capacity = 500 
                },
                new Location 
                { 
                    Name = "Tech Hub Lyon", 
                    Address = "45 Rue de la République", 
                    City = "Lyon", 
                    Country = "France", 
                    Capacity = 300 
                },
                new Location 
                { 
                    Name = "Innovation Campus", 
                    Address = "72 Boulevard des Innovations", 
                    City = "Bordeaux", 
                    Country = "France", 
                    Capacity = 200 
                }
            };
            
            await context.Locations.AddRangeAsync(locations);
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedRoomsAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des salles existent déjà
            if (await context.Rooms.AnyAsync())
                return;
            
            // Récupérez les lieux
            var locations = await context.Locations.ToListAsync();
            
            foreach (var location in locations)
            {
                // Créez des salles pour chaque lieu
                var rooms = new[]
                {
                    new Room { Name = $"Salle A - {location.Name}", Capacity = 100, LocationId = location.Id },
                    new Room { Name = $"Salle B - {location.Name}", Capacity = 80, LocationId = location.Id },
                    new Room { Name = $"Salle C - {location.Name}", Capacity = 50, LocationId = location.Id }
                };
                
                await context.Rooms.AddRangeAsync(rooms);
            }
            
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedEventsAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des événements existent déjà
            if (await context.Events.AnyAsync())
                return;
            
            // Récupérez les catégories et lieux
            var categories = await context.EventCategories.ToListAsync();
            var locations = await context.Locations.ToListAsync();
            
            if (!categories.Any() || !locations.Any())
                return;
            
            // Ajoutez des événements
            var events = new[]
            {
                new Event
                {
                    Title = "Conférence Tech 2025",
                    Description = "Conférence annuelle sur les dernières tendances technologiques",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(32),
                    Status = EventStatus.Published,
                    CategoryId = categories.First(c => c.Name == "Conférence").Id,
                    LocationId = locations.First(l => l.Name == "Centre de Conférences Paris").Id
                },
                new Event
                {
                    Title = "Workshop DevOps",
                    Description = "Atelier pratique sur les pratiques DevOps modernes",
                    StartDate = DateTime.UtcNow.AddDays(15),
                    EndDate = DateTime.UtcNow.AddDays(15).AddHours(8),
                    Status = EventStatus.Published,
                    CategoryId = categories.First(c => c.Name == "Workshop").Id,
                    LocationId = locations.First(l => l.Name == "Tech Hub Lyon").Id
                },
                new Event
                {
                    Title = "Formation Cloud Computing",
                    Description = "Formation complète sur les technologies cloud",
                    StartDate = DateTime.UtcNow.AddDays(45),
                    EndDate = DateTime.UtcNow.AddDays(47),
                    Status = EventStatus.Draft,
                    CategoryId = categories.First(c => c.Name == "Formation").Id,
                    LocationId = locations.First(l => l.Name == "Innovation Campus").Id
                }
            };
            
            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedParticipantsAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des participants existent déjà
            if (await context.Participants.AnyAsync())
                return;
            
            // Ajoutez des participants
            var participants = new[]
            {
                new Participant
                {
                    FirstName = "Jean",
                    LastName = "Dupont",
                    Email = "jean.dupont@example.com",
                    Company = "Tech Solutions",
                    JobTitle = "Développeur Senior"
                },
                new Participant
                {
                    FirstName = "Marie",
                    LastName = "Martin",
                    Email = "marie.martin@example.com",
                    Company = "Digital Agency",
                    JobTitle = "Chef de Projet"
                },
                new Participant
                {
                    FirstName = "Pierre",
                    LastName = "Leroy",
                    Email = "pierre.leroy@example.com",
                    Company = "Data Insights",
                    JobTitle = "Data Scientist"
                }
            };
            
            await context.Participants.AddRangeAsync(participants);
            await context.SaveChangesAsync();
            
            // Inscrivez les participants aux événements
            var events = await context.Events.ToListAsync();
            
            if (!events.Any())
                return;
            
            foreach (var participant in participants)
            {
                foreach (var eventEntity in events)
                {
                    await context.EventParticipants.AddAsync(new EventParticipant
                    {
                        EventId = eventEntity.Id,
                        ParticipantId = participant.Id,
                        RegistrationDate = DateTime.UtcNow.AddDays(-10),
                        AttendanceStatus = AttendanceStatus.Registered
                    });
                }
            }
            
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedSpeakersAsync(EventsAreasDbContext context)
        {
            // Vérifiez si des intervenants existent déjà
            if (await context.Speakers.AnyAsync())
                return;
            
            // Ajoutez des intervenants
            var speakers = new[]
            {
                new Speaker
                {
                    FirstName = "Sophie",
                    LastName = "Dubois",
                    Email = "sophie.dubois@example.com",
                    Company = "Tech Innovate",
                    Bio = "Experte en intelligence artificielle avec plus de 15 ans d'expérience dans le domaine."
                },
                new Speaker
                {
                    FirstName = "Thomas",
                    LastName = "Petit",
                    Email = "thomas.petit@example.com",
                    Company = "Cloud Systems",
                    Bio = "Architecte cloud spécialisé dans les déploiements à grande échelle."
                }
            };
            
            await context.Speakers.AddRangeAsync(speakers);
            await context.SaveChangesAsync();
            
            // Ajoutez des sessions et associez les intervenants
            var events = await context.Events.Include(e => e.Sessions).ToListAsync();
            var rooms = await context.Rooms.ToListAsync();
            
            if (!events.Any() || !rooms.Any())
                return;
            
            foreach (var eventEntity in events)
            {
                var sessionStart = eventEntity.StartDate.AddHours(1);
                
                var session = new Session
                {
                    Title = $"Session principale - {eventEntity.Title}",
                    Description = "Session d'ouverture de l'événement",
                    StartTime = sessionStart,
                    EndTime = sessionStart.AddHours(2),
                    EventId = eventEntity.Id,
                    RoomId = rooms.First().Id
                };
                
                await context.Sessions.AddAsync(session);
                await context.SaveChangesAsync();
                
                // Associez les intervenants à la session
                foreach (var speaker in speakers)
                {
                    await context.SessionSpeakers.AddAsync(new SessionSpeaker
                    {
                        SessionId = session.Id,
                        SpeakerId = speaker.Id,
                        Role = "Présentateur"
                    });
                }
            }
            
            await context.SaveChangesAsync();
        }
    }
}