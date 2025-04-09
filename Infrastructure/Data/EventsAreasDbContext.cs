using ISITECH__EventsArea.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Data
{
    public class EventsAreasDbContext : DbContext
    {
        public EventsAreasDbContext(DbContextOptions<EventsAreasDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SessionSpeaker> SessionSpeakers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des clés primaires composites
            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.ParticipantId });

            modelBuilder.Entity<SessionSpeaker>()
                .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

            // Configuration des relations
            
            // Event - EventCategory (Many-to-One)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event - Location (Many-to-One)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // EventParticipant - Event (Many-to-One)
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // EventParticipant - Participant (Many-to-One)
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Participant)
                .WithMany(p => p.EventParticipants)
                .HasForeignKey(ep => ep.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Session - Event (Many-to-One)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Event)
                .WithMany(e => e.Sessions)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Session - Room (Many-to-One)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Sessions)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // SessionSpeaker - Session (Many-to-One)
            modelBuilder.Entity<SessionSpeaker>()
                .HasOne(ss => ss.Session)
                .WithMany(s => s.SessionSpeakers)
                .HasForeignKey(ss => ss.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // SessionSpeaker - Speaker (Many-to-One)
            modelBuilder.Entity<SessionSpeaker>()
                .HasOne(ss => ss.Speaker)
                .WithMany(s => s.SessionSpeakers)
                .HasForeignKey(ss => ss.SpeakerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room - Location (Many-to-One)
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Location)
                .WithMany(l => l.Rooms)
                .HasForeignKey(r => r.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating - Session (Many-to-One)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Session)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating - Participant (Many-to-One)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Participant)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des propriétés
            modelBuilder.Entity<Event>()
                .Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Participant>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Participant>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Speaker>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Speaker>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Rating>()
                .Property(r => r.Score)
                .IsRequired()
                .HasColumnType("smallint");
        }
    }
}