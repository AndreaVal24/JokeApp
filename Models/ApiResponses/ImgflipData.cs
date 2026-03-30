
using System.Text.Json.Serialization;

namespace JokeApp.Models.ApiResponses
{
    /// <summary>
    /// Representa el objeto "data" dentro de la respuesta de Imgflip API.
    /// Contiene la lista de memes disponibles.
    /// Estructura JSON esperada:
    /// {
    ///   "data": {
    ///     "memes": [ { ... }, { ... } ]
    ///   }
    /// }
    /// </summary>
    public class ImgflipData
    {
        /// <summary>
        /// Lista de memes devueltos por la API.
        /// Imgflip retorna aproximadamente 100 memes populares por solicitud.
        /// </summary>
        [JsonPropertyName("memes")]
        public List<Meme> Memes { get; set; } = new List<Meme>();
    }
}