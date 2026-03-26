using JokeApp.Models.DTOs;

namespace JokeApp.Services.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones disponibles para obtener
    /// memes desde Imgflip API. Los ViewModels dependen de esta interfaz,
    /// no de la implementación concreta (MemeService).
    /// </summary>
    public interface IMemeService
    {
        /// <summary>
        /// Obtiene un meme aleatorio de la lista disponible en Imgflip API.
        /// </summary>
        /// <returns>
        /// Un <see cref="MemeDto"/> con el meme seleccionado aleatoriamente,
        /// o null si la API devuelve un error.
        /// </returns>
        Task<MemeDto?> GetRandomMemeAsync();

        /// <summary>
        /// Obtiene la lista completa de memes disponibles en Imgflip API.
        /// Imgflip retorna aproximadamente 100 memes populares por solicitud.
        /// </summary>
        /// <returns>
        /// Colección de <see cref="MemeDto"/> con todos los memes disponibles,
        /// o una colección vacía si la API devuelve un error.
        /// </returns>
        Task<IEnumerable<MemeDto>> GetAllMemesAsync();
    }
}