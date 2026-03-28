using Microsoft.EntityFrameworkCore;
using JokeApp.Models;
using JokeApp.Models.ApiResponses;

namespace JokeApp.Data
{
    public class AppDbContext : DbContext
    {
        // public DbSet<Favorite> Favorites { get; set; } — pendiente Persona 2

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
        }
    }
}