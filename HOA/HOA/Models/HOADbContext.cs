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
        // Remove DbSet<User> if using IdentityUser for authentication
    }
}
