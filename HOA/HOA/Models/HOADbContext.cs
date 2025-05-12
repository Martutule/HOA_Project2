using Microsoft.EntityFrameworkCore;
using HOA.Models;

namespace HOA.Models
{
    public class HOADbContext : DbContext
    {
        public HOADbContext(DbContextOptions<HOADbContext> options)
            : base(options)
        {
        }

        public DbSet<Resident> Residents { get; set; } = default!;

        public DbSet<Payment> Payments { get; set; } = default!;

        public DbSet<Event> Events { get; set; } = default!;    

        public DbSet<Maintenance> Maintenances { get; set; } = default!;

        public DbSet<Announcement> Announcements { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;
    }
}
