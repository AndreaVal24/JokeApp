using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokeApp.Models
{
    public class HistoryItem
    {
        // ─── Propiedades ───────────────────────────────────────────────────────

        /// <summary>
        /// Identificador único del registro.
        /// EF Core lo usa como clave primaria (PRIMARY KEY AUTOINCREMENT).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tipo de contenido generado.
        /// Valores válidos: <c>"joke"</c> o <c>"meme"</c>.
        /// Máximo 10 caracteres (restricción definida en AppDbContext).
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del contenido proveniente de la API externa.
        /// Para chistes: ID de JokeAPI. Para memes: ID de Imgflip.
        /// Máximo 50 caracteres.
        /// </summary>
        public string ContentId { get; set; } = string.Empty;

        /// <summary>
        /// Título o descripción breve del contenido generado.
        /// Se muestra como texto principal en la vista de historial.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora exacta en que se generó el contenido.
        /// Se usa para ordenar el historial del más reciente al más antiguo.
        /// </summary>
        public DateTime GeneratedAt { get; set; }
    }
}
