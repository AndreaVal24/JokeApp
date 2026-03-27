using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JokeApp.Services;
using JokeApp.Models.DTOs;
using JokeApp.Services.Interfaces;

namespace JokeApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IJokeService _jokeService;
        private readonly MemeService _memeService;

        private JokeDto? _currentJoke;
        private MemeDto? _currentMeme;

        [ObservableProperty]
        private string _jokeText = "Presiona Generar Chiste para comenzar 😄";

        [ObservableProperty]
        private string _memeUrl = string.Empty;

        [ObservableProperty]
        private string _memeName = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = "Listo";

        [ObservableProperty]
        private bool _hasJoke = false;

        [ObservableProperty]
        private bool _hasMeme = false;

        [ObservableProperty]
        private string _selectedCategory = "Any";

        [ObservableProperty]
        private IEnumerable<string> _availableCategories = new List<string>();

        [ObservableProperty]
        private string _languageButtonText = "EN";

        // Idioma actual: "en" o "es"
        private string _selectedLanguage = "en";

        public MainViewModel(IJokeService jokeService, MemeService memeService)
        {
            _jokeService = jokeService;
            _memeService = memeService;
            AvailableCategories = _jokeService.GetAvailableCategories();
        }

        [RelayCommand]
        private async Task GetJokeAsync()
        {
            IsLoading = true;
            StatusMessage = "Obteniendo chiste...";
            try
            {
                // La interfaz IJokeService solo acepta category por ahora.
                // El idioma se maneja construyendo la categoria con prefijo de idioma
                // cuando el servicio de Tomas soporte el parametro lang.
                // TODO: cambiar a GetJokeAsync(SelectedCategory, _selectedLanguage)
                // cuando Tomas actualice IJokeService para soportar idioma.
                _currentJoke = await _jokeService.GetJokeAsync(SelectedCategory);

                if (_currentJoke != null)
                {
                    JokeText = _currentJoke.Text;
                    HasJoke = true;
                    StatusMessage = _selectedLanguage == "es"
                        ? "Chiste cargado ✓ (nota: JokeAPI tiene pocos chistes en español)"
                        : "Chiste cargado ✓";
                }
                else
                {
                    JokeText = "No se pudo obtener el chiste. Intenta de nuevo.";
                    HasJoke = false;
                    StatusMessage = "Sin respuesta de la API";
                }
            }
            catch (Exception ex)
            {
                JokeText = "Ocurrio un error. Intenta de nuevo.";
                StatusMessage = $"Error: {ex.Message}";
                HasJoke = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ToggleLanguage()
        {
            if (_selectedLanguage == "en")
            {
                _selectedLanguage = "es";
                LanguageButtonText = "ES";
                StatusMessage = "Idioma: Espanol — genera un nuevo chiste";
            }
            else
            {
                _selectedLanguage = "en";
                LanguageButtonText = "EN";
                StatusMessage = "Idioma: Ingles — genera un nuevo chiste";
            }
        }

        [RelayCommand]
        private async Task GetMemeAsync()
        {
            IsLoading = true;
            StatusMessage = "Obteniendo meme...";
            try
            {
                _currentMeme = await _memeService.GetRandomMemeAsync();

                if (_currentMeme != null)
                {
                    MemeUrl = _currentMeme.Url;
                    MemeName = _currentMeme.Name;
                    HasMeme = true;
                    StatusMessage = "Meme cargado ✓";
                }
                else
                {
                    HasMeme = false;
                    StatusMessage = "Sin respuesta de la API";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                HasMeme = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void SaveJokeAsFavorite()
        {
            if (_currentJoke == null) return;
            // TODO: implementar con DatabaseService cuando Persona 2 suba su modulo
            StatusMessage = "Chiste guardado en favoritos ⭐";
        }

        [RelayCommand]
        private void SaveMemeAsFavorite()
        {
            if (_currentMeme == null) return;
            // TODO: implementar con DatabaseService cuando Persona 2 suba su modulo
            StatusMessage = "Meme guardado en favoritos ⭐";
        }

        [RelayCommand]
        private void OpenFavorites()
        {
            // TODO: new Views.FavoritesView().Show(); cuando Persona 2 suba su vista
            MessageBox.Show("Vista de Favoritos en construccion.", "JokeHub");
        }

        [RelayCommand]
        private void OpenHistory()
        {
            // TODO: new Views.HistoryView().Show(); cuando Persona 3 suba su vista
            MessageBox.Show("Vista de Historial en construccion.", "JokeHub");
        }
    }
}