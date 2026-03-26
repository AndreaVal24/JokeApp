using JokeApp.Models.ApiResponses;
using JokeApp.Models.DTOs;
using JokeAppw.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace HumorApp.Services
{
    /// <summary>
    /// Implementación concreta de <see cref="IMemeService"/>.
    /// Consume Imgflip API, deserializa la respuesta cruda y
    /// la mapea a <see cref="MemeDto"/> para uso interno de la app.
    /// </summary>
    public class MemeService : IMemeService
    {
        // ─── Constantes ────────────────────────────────────────────────────────

        /// <summary>
        /// URL del endpoint de Imgflip que retorna los memes disponibles.
        /// </summary>
        private const string ApiUrl = "https://api.imgflip.com/get_memes";

        // ─── Dependencias ──────────────────────────────────────────────────────

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly Random _random = new Random();

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el servicio con un <see cref="HttpClient"/> inyectado.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP provisto por el contenedor de DI.</param>
        public MemeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ─── Métodos públicos ──────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task<MemeDto?> GetRandomMemeAsync()
        {
            var memes = (await GetAllMemesAsync()).ToList();

            if (!memes.Any())
                return null;

            var index = _random.Next(memes.Count);
            return memes[index];
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MemeDto>> GetAllMemesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var raw = await JsonSerializer.DeserializeAsync<ImgflipResponse>(stream, _jsonOptions);

                if (raw == null || !raw.Success || raw.Data == null)
                    return Enumerable.Empty<MemeDto>();

                return raw.Data.Memes.Select(MapToDto);
            }
            catch (HttpRequestException)
            {
                // Error de red o respuesta HTTP no exitosa
                return Enumerable.Empty<MemeDto>();
            }
            catch (JsonException)
            {
                // Error al deserializar la respuesta
                return Enumerable.Empty<MemeDto>();
            }
        }

        // ─── Mapeo privado ─────────────────────────────────────────────────────

        /// <summary>
        /// Mapea un <see cref="Meme"/> crudo a un <see cref="MemeDto"/> listo para la UI.
        /// </summary>
        /// <param name="raw">Objeto crudo deserializado de Imgflip API.</param>
        /// <returns>DTO limpio con solo los campos necesarios para la app.</returns>
        private static MemeDto MapToDto(Meme raw)
        {
            return new MemeDto
            {
                Id = raw.Id,
                Name = raw.Name,
                Url = raw.Url,
                FetchedAt = DateTime.Now
            };
        }
    }
}