namespace JokeApp.Models.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) que representa un meme procesado
    /// y listo para ser consumido por los ViewModels y la UI.
    /// Esta clase es independiente de la estructura de Imgflip API.
    /// </summary>
    public class MemeDto
    {
        /// <summary>
        /// Identificador único del meme.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo del meme (ej: "Drake Hotline Bling").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL pública de la imagen del meme, lista para bindear
        /// directamente a un control Image en WPF.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que fue obtenido el meme.
        /// Útil para el historial y favoritos.
        /// </summary>
        public DateTime FetchedAt { get; set; } = DateTime.Now;
    }
}