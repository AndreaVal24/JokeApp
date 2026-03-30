using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JokeApp.Models;
using JokeApp.Data;
using Microsoft.EntityFrameworkCore;


namespace JokeApp.Services
{
    /// <summary>
    /// Servicio responsable de gestionar el historial de contenido generado.
    /// Actúa como intermediario entre el <see cref="AppDbContext"/> y el
    /// <see cref="JokeApp.ViewModels.HistoryViewModel"/>, siguiendo el principio
    /// de separación de responsabilidades (SOLID — SRP).
    /// </summary>
    /// <remarks>
    /// Este servicio es instanciado manualmente en <c>MainWindow.xaml.cs</c>
    /// junto al resto de servicios de la aplicación, ya que el proyecto
    /// no usa un contenedor de inyección de dependencias automático.
    /// </remarks>
    public class HistoryService
    {
        // ─── Dependencias ──────────────────────────────────────────────────────

        /// <summary>
        /// Contexto de Entity Framework Core que representa la base de datos SQLite.
        /// Provisto por Persona 4 (Tomás). Se usa para leer y escribir en la
        /// tabla <c>HistoryItems</c>.
        /// </summary>
        private readonly AppDbContext _context;

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el servicio con el contexto de base de datos.
        /// </summary>
        /// <param name="context">
        /// Instancia de <see cref="AppDbContext"/> inyectada manualmente
        /// desde <c>MainWindow.xaml.cs</c>.
        /// </param>
        public HistoryService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Métodos públicos ──────────────────────────────────────────────────

        /// <summary>
        /// Agrega un nuevo registro al historial en la base de datos.
        /// Es llamado por <c>MainViewModel</c> cada vez que se genera
        /// un chiste o meme exitosamente.
        /// </summary>
        /// <param name="contentType">
        /// Tipo de contenido: <c>"joke"</c> o <c>"meme"</c>.
        /// Máximo 10 caracteres (restricción definida en AppDbContext).
        /// </param>
        /// <param name="contentId">
        /// Identificador del contenido en la API externa.
        /// Para chistes: ID numérico de JokeAPI.
        /// Para memes: ID string de Imgflip.
        /// Máximo 50 caracteres.
        /// </param>
        /// <param name="title">
        /// Texto descriptivo para mostrar en la vista de historial.
        /// Para chistes: primeras palabras del chiste.
        /// Para memes: nombre del meme.
        /// </param>
        public async Task AddAsync(string contentType, string contentId, string title)
        {
            var item = new HistoryItem
            {
                ContentType = contentType,
                ContentId = contentId,
                Title = title,
                GeneratedAt = DateTime.Now
            };

            _context.HistoryItems.Add(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene todos los registros del historial ordenados del más reciente
        /// al más antiguo.
        /// </summary>
        /// <returns>
        /// Lista de <see cref="HistoryItem"/> ordenada por <c>GeneratedAt</c>
        /// de forma descendente. Lista vacía si no hay registros.
        /// </returns>
        public async Task<List<HistoryItem>> GetAllAsync()
        {
            return await _context.HistoryItems
                .OrderByDescending(h => h.GeneratedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Elimina todos los registros del historial de la base de datos.
        /// Esta acción es irreversible.
        /// </summary>
        public async Task ClearAllAsync()
        {
            _context.HistoryItems.RemoveRange(_context.HistoryItems);
            await _context.SaveChangesAsync();
        }
    }
}
