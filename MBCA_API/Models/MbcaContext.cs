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
            builder.Entity<EventBanner>().HasOne(f => f.Event).WithMany(f => f.eventBanners);
            builder.Entity<Event>().HasOne(f => f.eventCategory).WithMany(f => f.events);
            builder.Entity<ExhibitTag>().HasOne(f => f.exhibit).WithMany(f => f.exhibitTags);
            builder.Entity<Exhibit>().HasOne(f => f.exhibitCategory).WithMany(f => f.exhibits);
            builder.Entity<OTP>().HasOne(f => f.user).WithMany(f => f.otps);
            builder.Entity<Ticket>(ee =>
            {
                ee.HasOne(f => f.promo).WithMany(f => f.tickets);
                ee.HasOne(f => f.Event).WithMany(f => f.tickets);
                ee.HasOne(f => f.user).WithMany(f => f.tickets);
            });
            builder.Entity<EventExhibit>(ee =>
            {
                ee.HasOne(f => f.Event).WithMany(f => f.eventExhibits);
                ee.HasOne(f => f.exhibit).WithMany(f => f.eventExhibits);
            });
        }
    }
}
