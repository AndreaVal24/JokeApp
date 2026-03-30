using System.Text.Json.Serialization;

namespace JokeApp.Models.ApiResponses
{
    /// <summary>
    /// Representa un meme individual dentro de la respuesta cruda de Imgflip API.
    /// Documentación: https://api.imgflip.com/
    /// </summary>
    public class Meme
    {
        /// <summary>
        /// Identificador único del meme en Imgflip.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo del meme (ej: "Drake Hotline Bling").
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL pública de la imagen del meme.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Ancho de la imagen en píxeles.
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Alto de la imagen en píxeles.
        /// </summary>
        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// Cantidad de cuadros de texto que soporta el meme.
        /// </summary>
        [JsonPropertyName("box_count")]
        public int BoxCount { get; set; }
    }
}