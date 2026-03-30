namespace JokeApp.Models.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) que representa un chiste procesado
    /// y listo para ser consumido por los ViewModels y la UI.
    /// Esta clase es independiente de la estructura de JokeAPI.
    /// </summary>
    public class JokeDto
    {
        /// <summary>
        /// Identificador único del chiste.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Categoría del chiste (ej: "Programming", "Misc", "Dark").
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Texto completo del chiste listo para mostrar en UI.
        /// Si el chiste era "twopart", contiene Setup y Delivery unidos.
        /// Formato: "Setup ... Delivery"
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Código de idioma del chiste (ej: "en", "es").
        /// </summary>
        public string Lang { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que fue generado/obtenido el chiste.
        /// Útil para el historial.
        /// </summary>
        public DateTime FetchedAt { get; set; } = DateTime.Now;
    }
}