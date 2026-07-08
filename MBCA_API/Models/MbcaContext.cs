using Microsoft.EntityFrameworkCore;

namespace MBCA_API.Models
{
    public class MBCAContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Promo> Promos { get; set; }
        public DbSet<PhonePrefix> PhonePrefixes { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<ExhibitTag> ExhibitTags { get; set; }
        public DbSet<ExhibitCategory> ExhibitCategories { get; set; }
        public DbSet<Exhibit> Exhibits { get; set; }
        public DbSet<EventExhibit> EventExhibits { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventBanner> EventBanners { get; set; }
        public DbSet<Event> Events { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Integrated Security=true;Database=MBCA");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventBanner>().HasOne(f => f.Event).WithMany(f => f.eventBanners).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Event>().HasOne(f => f.eventCategory).WithMany(f => f.events).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ExhibitTag>().HasOne(f => f.exhibit).WithMany(f => f.exhibitTags).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Exhibit>().HasOne(f => f.exhibitCategory).WithMany(f => f.exhibits).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OTP>().HasOne(f => f.user).WithMany(f => f.otps).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ticket>(ee =>
            {
                ee.HasOne(f => f.promo).WithMany(f => f.tickets).OnDelete(DeleteBehavior.ClientSetNull);
                ee.HasOne(f => f.Event).WithMany(f => f.tickets).OnDelete(DeleteBehavior.ClientSetNull);
                ee.HasOne(f => f.user).WithMany(f => f.tickets).OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<EventExhibit>(ee =>
            {
                ee.HasOne(f => f.Event).WithMany(f => f.eventExhibits).OnDelete(DeleteBehavior.Cascade);
                ee.HasOne(f => f.exhibit).WithMany(f => f.eventExhibits).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
