using System.Text.Json.Serialization;

namespace JokeApp.Models.ApiResponses
{
    /// <summary>
    /// Representa la respuesta cruda de JokeAPI.
    /// Soporta dos tipos de chiste: "single" y "twopart".
    /// Documentación: https://v2.jokeapi.dev/
    /// </summary>
    public class Joke
    {
        /// <summary>
        /// Identificador único del chiste en JokeAPI.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Categoría del chiste (ej: "Programming", "Misc", "Dark").
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de chiste: "single" o "twopart".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Texto del chiste completo. Solo presente si Type == "single".
        /// </summary>
        [JsonPropertyName("joke")]
        public string? JokeText { get; set; }

        /// <summary>
        /// Primera parte del chiste (la pregunta). Solo presente si Type == "twopart".
        /// </summary>
        [JsonPropertyName("setup")]
        public string? Setup { get; set; }

        /// <summary>
        /// Segunda parte del chiste (la respuesta). Solo presente si Type == "twopart".
        /// </summary>
        [JsonPropertyName("delivery")]
        public string? Delivery { get; set; }

        /// <summary>
        /// Código de idioma del chiste (ej: "en", "es").
        /// </summary>
        [JsonPropertyName("lang")]
        public string Lang { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el chiste es apto para todo público.
        /// </summary>
        [JsonPropertyName("safe")]
        public bool Safe { get; set; }

        /// <summary>
        /// Indica si la respuesta de la API contiene un error.
        /// </summary>
        [JsonPropertyName("error")]
        public bool Error { get; set; }
    }
}