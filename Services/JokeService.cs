using JokeApp.Models.ApiResponses;
using JokeApp.Models.DTOs;
using JokeApp.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace JokeApp.Services
{
    /// <summary>
    /// Implementación concreta de <see cref="IJokeService"/>.
    /// Consume JokeAPI v2, deserializa la respuesta cruda y
    /// la mapea a <see cref="JokeDto"/> para uso interno de la app.
    /// </summary>
    public class JokeService : IJokeService
    {
        // ─── Constantes ────────────────────────────────────────────────────────

        /// <summary>
        /// URL base de JokeAPI v2.
        /// </summary>
        private const string BaseUrl = "https://v2.jokeapi.dev/joke";

        /// <summary>
        /// Categorías disponibles en JokeAPI.
        /// "Any" es un comodín que incluye todas las categorías.
        /// </summary>
        private static readonly IEnumerable<string> Categories = new List<string>
        {
            "Any",
            "Programming",
            "Misc",
            "Dark",
            "Pun",
            "Spooky",
            "Christmas"
        };

        // ─── Dependencias ──────────────────────────────────────────────────────

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el servicio con un <see cref="HttpClient"/> inyectado.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP provisto por el contenedor de DI.</param>
        public JokeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ─── Métodos públicos ──────────────────────────────────────────────────

        /// <inheritdoc/>
        public IEnumerable<string> GetAvailableCategories()
        {
            return Categories;
        }

        /// <inheritdoc/>
        public async Task<JokeDto?> GetJokeAsync(string category)
        {
            try
            {
                var url = $"{BaseUrl}/{Uri.EscapeDataString(category)}?type=single,twopart";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var raw = await JsonSerializer.DeserializeAsync<Joke>(stream, _jsonOptions);

                if (raw == null || raw.Error)
                    return null;

                return MapToDto(raw);
            }
            catch (HttpRequestException)
            {
                // Error de red o respuesta HTTP no exitosa
                return null;
            }
            catch (JsonException)
            {
                // Error al deserializar la respuesta
                return null;
            }
        }

        // ─── Mapeo privado ─────────────────────────────────────────────────────

        /// <summary>
        /// Mapea un <see cref="Joke"/> crudo a un <see cref="JokeDto"/> listo para la UI.
        /// Resuelve la diferencia entre chistes "single" y "twopart".
        /// </summary>
        /// <param name="raw">Objeto crudo deserializado de JokeAPI.</param>
        /// <returns>DTO limpio con el texto del chiste ya resuelto.</returns>
        private static JokeDto MapToDto(Joke raw)
        {
            var text = raw.Type == "twopart"
                ? $"{raw.Setup}\n\n{raw.Delivery}"
                : raw.JokeText ?? string.Empty;

            return new JokeDto
            {
                Id = raw.Id,
                Category = raw.Category,
                Text = text,
                Lang = raw.Lang,
                FetchedAt = DateTime.Now
            };
        }
    }
}