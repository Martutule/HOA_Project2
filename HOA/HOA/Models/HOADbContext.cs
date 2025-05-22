using Microsoft.EntityFrameworkCore;
using HOA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HOA.Models
{
    public class HOADbContext : IdentityDbContext<IdentityUser>
    {
        public HOADbContext(DbContextOptions<HOADbContext> options)
            : base(options)
        {
        }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        // Remove DbSet<User> if using IdentityUser for authentication
        public DbSet<SupplierContract> SupplierContracts { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important for Identity

            // Configure the Event relationship instead
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany()
                .HasForeignKey(ep => ep.EventId);

            // Optional: Configure the foreign key to IdentityUser without navigation property
            modelBuilder.Entity<EventParticipant>()
                .HasIndex(ep => new { ep.EventId, ep.UserId })
                .IsUnique(); // Prevent duplicate participations
        }
    }
}