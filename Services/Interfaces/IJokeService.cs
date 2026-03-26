using JokeApp.Models.DTOs;


namespace JokeApp.Services.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones disponibles para obtener
    /// chistes desde JokeAPI. Los ViewModels dependen de esta interfaz,
    /// no de la implementación concreta (JokeService).
    /// </summary>
    public interface IJokeService
    {
        /// <summary>
        /// Obtiene un chiste aleatorio de JokeAPI según la categoría indicada.
        /// </summary>
        /// <param name="category">
        /// Categoría del chiste. Valores válidos: "Any", "Programming",
        /// "Misc", "Dark", "Pun", "Spooky", "Christmas".
        /// </param>
        /// <returns>
        /// Un <see cref="JokeDto"/> con el chiste procesado,
        /// o null si la API devuelve un error.
        /// </returns>
        Task<JokeDto?> GetJokeAsync(string category);

        /// <summary>
        /// Devuelve la lista de categorías disponibles en JokeAPI.
        /// Utilizado por la UI para poblar el ComboBox de categorías.
        /// </summary>
        /// <returns>
        /// Colección de strings con los nombres de las categorías.
        /// </returns>
        IEnumerable<string> GetAvailableCategories();
    }
}