using Microsoft.EntityFrameworkCore;
using JokeApp.Models;
namespace JokeApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<HistoryItem> HistoryItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HistoryItem>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.ContentType)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(h => h.ContentId)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(h => h.GeneratedAt)
                      .IsRequired();
                entity.HasIndex(h => h.GeneratedAt);
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.ContentType)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(f => f.ContentId)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(f => f.Title)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(f => f.Content)
                      .HasMaxLength(4000);
                entity.Property(f => f.ImageUrl)
                      .HasMaxLength(500);
                entity.Property(f => f.SavedAt)
                      .IsRequired();
                entity.HasIndex(f => f.SavedAt);
                entity.HasIndex(f => new { f.ContentType, f.ContentId })
                      .IsUnique();
            });
        }
    }
}
