using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MBCA_API.Models;

public partial class MbcaContext : DbContext
{
    public MbcaContext()
    {
    }

    public MbcaContext(DbContextOptions<MbcaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventBanner> EventBanners { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<EventExhibit> EventExhibits { get; set; }

    public virtual DbSet<Exhibit> Exhibits { get; set; }

    public virtual DbSet<ExhibitCategory> ExhibitCategories { get; set; }

    public virtual DbSet<ExhibitTag> ExhibitTags { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<PhonePrefix> PhonePrefixes { get; set; }

    public virtual DbSet<Promo> Promos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Database=MBCA");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC27F6AE7628");

            entity.ToTable("Event");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EventCategoryId).HasColumnName("EventCategoryID");
            entity.Property(e => e.Initiator)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.EventCategory).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_Category");
        });

        modelBuilder.Entity<EventBanner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventBan__3214EC27CA797911");

            entity.ToTable("EventBanner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Banner).HasColumnType("text");
            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventBanners)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventBanner_Event");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventCat__3214EC2726903DA5");

            entity.ToTable("EventCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EventExhibit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventExh__3214EC277143BAB2");

            entity.ToTable("EventExhibit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ExhibitId).HasColumnName("ExhibitID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventExhibits)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventExhibit_Event");

            entity.HasOne(d => d.Exhibit).WithMany(p => p.EventExhibits)
                .HasForeignKey(d => d.ExhibitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventExhibit_Exhibit");
        });

        modelBuilder.Entity<Exhibit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exhibit__3214EC27DC2D277D");

            entity.ToTable("Exhibit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Artist)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ExhibitCategoryId).HasColumnName("ExhibitCategoryID");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TimePeriod)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.ExhibitCategory).WithMany(p => p.Exhibits)
                .HasForeignKey(d => d.ExhibitCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exhibit_ExhibitCategory");
        });

        modelBuilder.Entity<ExhibitCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExhibitC__3214EC2725120D6F");

            entity.ToTable("ExhibitCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExhibitTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExhibitT__3214EC275C91F8D3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ExhibitId).HasColumnName("ExhibitID");
            entity.Property(e => e.Tag)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Exhibit).WithMany(p => p.ExhibitTags)
                .HasForeignKey(d => d.ExhibitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExhibitTags_Exhibit");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OTP__3214EC272CE1A327");

            entity.ToTable("OTP");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ValidUntil)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.User).WithMany(p => p.Otps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OTP_User");
        });

        modelBuilder.Entity<PhonePrefix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhonePre__3214EC272EBA71C7");

            entity.ToTable("PhonePrefix");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Prefix)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Promo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promo__3214EC27334ADF4D");

            entity.ToTable("Promo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC273298B36F");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3214EC27699F82C2");

            entity.ToTable("Ticket");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.PromoId).HasColumnName("PromoID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Event");

            entity.HasOne(d => d.Promo).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PromoId)
                .HasConstraintName("FK_Ticket_Promo");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2703BE47A1");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
