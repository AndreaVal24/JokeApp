using Microsoft.EntityFrameworkCore;

// Estas entidades son desarrolladas por Persona 2 y Persona 3.
// Se referencian aquí solo para registrar los DbSets en el contexto.
using JokeApp.Models.ApiResponses;

namespace JokeApp.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework Core para la aplicación.
    /// Gestiona la conexión con la base de datos SQLite local (app.db)
    /// y expone los DbSets que representan las tablas del sistema.
    /// </summary>
    public class AppDbContext : DbContext
    {
        // ─── DbSets (Tablas) ───────────────────────────────────────────────────

        /// <summary>
        /// Tabla de favoritos guardados por el usuario.
        /// Entidad desarrollada por Persona 2.
        /// </summary>
        public DbSet<Favorite> Favorites { get; set; }

        /// <summary>
        /// Tabla del historial de contenido generado.
        /// Entidad desarrollada por Persona 3.
        /// </summary>
        public DbSet<HistoryItem> HistoryItems { get; set; }

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el contexto con las opciones provistas por el contenedor de DI.
        /// </summary>
        /// <param name="options">Opciones de configuración inyectadas (conexión SQLite).</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ─── Configuración ─────────────────────────────────────────────────────

        /// <summary>
        /// Configura el modelo de base de datos mediante Fluent API.
        /// Define restricciones, índices y relaciones entre entidades.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Configuración de Favorite ──────────────────────────────────────
            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.ContentType)
                      .IsRequired()
                      .HasMaxLength(10);     // "joke" | "meme"

                entity.Property(f => f.ContentId)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(f => f.Title)
                      .HasMaxLength(200);

                entity.Property(f => f.SavedAt)
                      .IsRequired();
            });

            // ── Configuración de HistoryItem ───────────────────────────────────
            modelBuilder.Entity<HistoryItem>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.ContentType)
                      .IsRequired()
                      .HasMaxLength(10);     // "joke" | "meme"

                entity.Property(h => h.ContentId)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(h => h.GeneratedAt)
                      .IsRequired();

                // Índice para acelerar consultas por fecha
                entity.HasIndex(h => h.GeneratedAt);
            });
        }
    }
}