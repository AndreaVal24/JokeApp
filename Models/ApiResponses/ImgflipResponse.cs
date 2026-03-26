using System.Text.Json.Serialization;

namespace JokeApp.Models.ApiResponses
{
    /// <summary>
    /// Representa la respuesta raíz de la API de Imgflip.
    /// Estructura JSON esperada:
    /// {
    ///   "success": true,
    ///   "data": { ... }
    /// }
    /// Documentación: https://api.imgflip.com/
    /// </summary>
    public class ImgflipResponse
    {
        /// <summary>
        /// Indica si la solicitud a la API fue exitosa.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Contenedor principal de los datos devueltos por la API.
        /// Solo tiene valor si Success == true.
        /// </summary>
        [JsonPropertyName("data")]
        public ImgflipData? Data { get; set; }
    }
}