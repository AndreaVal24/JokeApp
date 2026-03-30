using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JokeApp.Data;
using JokeApp.Models.DTOs;
using JokeApp.Services;
using JokeApp.Services.Interfaces;

namespace JokeApp.ViewModels
{
    /// <summary>
    /// ViewModel principal de la aplicación.
    /// Gestiona la generación de chistes y memes, y coordina
    /// la navegación hacia las vistas de Historial y Favoritos.
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        // ─── Dependencias ──────────────────────────────────────────────────────
        private readonly IJokeService _jokeService;
        private readonly IMemeService _memeService;
        private readonly HistoryService _historyService;
        private readonly DatabaseService _databaseService;
        private readonly AppDbContext _context;

        private JokeDto? _currentJoke;
        private MemeDto? _currentMeme;

        // ─── Propiedades observables ───────────────────────────────────────────

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

        private string _selectedLanguage = "en";

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el ViewModel con los servicios necesarios.
        /// </summary>
        public MainViewModel(
            IJokeService jokeService,
            IMemeService memeService,
            HistoryService historyService,
            DatabaseService databaseService,
            AppDbContext context)
        {
            _jokeService = jokeService;
            _memeService = memeService;
            _historyService = historyService;
            _databaseService = databaseService;
            _context = context;
            AvailableCategories = _jokeService.GetAvailableCategories();
        }

        // ─── Comandos ──────────────────────────────────────────────────────────

        [RelayCommand]
        private async Task GetJokeAsync()
        {
            IsLoading = true;
            StatusMessage = "Obteniendo chiste...";
            try
            {
                _currentJoke = await _jokeService.GetJokeAsync(SelectedCategory, _selectedLanguage);

                if (_currentJoke != null)
                {
                    JokeText = _currentJoke.Text;
                    HasJoke = true;
                    StatusMessage = "Chiste cargado ✓";
                    await _historyService.AddAsync("joke", _currentJoke.Id.ToString(), _currentJoke.Text);
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
                StatusMessage = "Idioma: Español — genera un nuevo chiste";
            }
            else
            {
                _selectedLanguage = "en";
                LanguageButtonText = "EN";
                StatusMessage = "Idioma: Inglés — genera un nuevo chiste";
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
                    await _historyService.AddAsync("meme", _currentMeme.Id, _currentMeme.Name);
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

            _ = SaveJokeFavoriteAsync();
        }

        [RelayCommand]
        private void SaveMemeAsFavorite()
        {
            if (_currentMeme == null) return;

            _ = SaveMemeFavoriteAsync();
        }

        [RelayCommand]
        private void OpenFavorites()
        {
            new global::JokeApp.Views.FavoritesView(_context).Show();
        }

        [RelayCommand]
        private void OpenHistory()
        {
            new global::JokeApp.Views.HistoryView(_context).Show();
        }

        private async Task SaveJokeFavoriteAsync()
        {
            try
            {
                var isNew = await _databaseService.SaveJokeAsync(_currentJoke!);
                StatusMessage = isNew
                    ? "Chiste guardado en favoritos ⭐"
                    : "Chiste ya estaba en favoritos";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar favorito: {ex.Message}";
            }
        }

        private async Task SaveMemeFavoriteAsync()
        {
            try
            {
                var isNew = await _databaseService.SaveMemeAsync(_currentMeme!);
                StatusMessage = isNew
                    ? "Meme guardado en favoritos ⭐"
                    : "Meme ya estaba en favoritos";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar favorito: {ex.Message}";
            }
        }
    }
}
