using DIG103_Ticket_platform_back.Model;
using Microsoft.EntityFrameworkCore;

namespace DIG103_Ticket_platform_back.Data;

public class AppliactionDbContext : DbContext
{
    public AppliactionDbContext(DbContextOptions<AppliactionDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventFeature> Features { get; set; }
    public DbSet<EventTheme> Themes { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ContentType).HasMaxLength(50);
            entity.Property(e => e.UploadedAt).IsRequired();
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsFeatured).IsRequired();

        });

        modelBuilder.Entity<EventTheme>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ColorPrimary).HasMaxLength(20);
            entity.Property(e => e.ColorPrimaryLight).HasMaxLength(20);
            entity.Property(e => e.ColorSecondary).HasMaxLength(20);
            entity.Property(e => e.FontFamily).HasMaxLength(100);
            entity.HasIndex(e => e.EventId).IsUnique();
        });

        modelBuilder.Entity<EventFeature>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => e.EventId);
        });

        modelBuilder.Entity<Event>()
            .HasMany(a => a.Artists)
            .WithMany(e => e.Events);

        modelBuilder.Entity<Event>()
            .HasOne(t => t.Theme)
            .WithOne(e => e.Event)
            .HasForeignKey<EventTheme>(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Event>()
            .HasMany(f => f.Features)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Artist>()
            .HasOne(a => a.ArtistImage)
            .WithOne()
            .HasForeignKey<Artist>("ArtistImageId")
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Event>()
            .HasOne(e => e.EventImage)
            .WithOne()
            .HasForeignKey<Event>("EventImageId")
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.EventBackground)
            .WithOne()
            .HasForeignKey<Event>("EventBackgroundId")
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<EventFeature>()
            .HasOne(ef => ef.FeatureImage)
            .WithOne()
            .HasForeignKey<EventFeature>("FeatureImageId")
            .OnDelete(DeleteBehavior.SetNull);
            
        /*SeedData(modelBuilder);*/
    }

    /*private void SeedData(ModelBuilder modelBuilder)
    {
        
    }*/
}