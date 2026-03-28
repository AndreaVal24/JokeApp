using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JokeApp.Data;
using JokeApp.Models.DTOs;
using JokeApp.Models;
using JokeApp.Models.DTOs;
using JokeApp.Services;
using JokeApp.Services.Interfaces;
using JokeApp.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
 

namespace JokeApp.ViewModels
{
    // ObservableObject viene de CommunityToolkit.Mvvm y reemplaza INotifyPropertyChanged
    public partial class MainViewModel : ObservableObject
    {
        // ─── Servicios inyectados desde el constructor ───────────────────────
        private readonly JokeService _jokeService;
        private readonly MemeService _memeService;
        private readonly HistoryService _historyService;
        private readonly HistoryService _historyService;
        private readonly DatabaseService _databaseService;

        // Guardamos el último joke/meme para usarlos en "Guardar favorito"
        private Joke? _currentJoke;
        private Meme? _currentMeme;

        // ─── Propiedades observables (source-generated por [ObservableProperty]) ───
        // [ObservableProperty] genera automáticamente la propiedad pública JokeText
        // con su setter que llama a OnPropertyChanged. NO escribas el getter/setter manualmente.

        [ObservableProperty]
        private string _jokeText = "¡Presiona 'Generar Chiste' para comenzar! 😄";

        [ObservableProperty]
        private string _memeUrl = string.Empty;

        [ObservableProperty]
        private string _memeName = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = "Listo";

        // Controla si los botones "Guardar en Favoritos" están habilitados
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

        public MainViewModel(IJokeService jokeService, MemeService memeService, HistoryService historyService)
        // ─── Constructor ──────────────────────────────────────────────────────
        public MainViewModel(
            JokeService jokeService,
            MemeService memeService,
            HistoryService historyService,
            DatabaseService databaseService)
        {
            _jokeService = jokeService;
            _memeService = memeService;
            _historyService = historyService;
            AvailableCategories = _jokeService.GetAvailableCategories();
            _historyService = historyService;
            _databaseService = databaseService;
        }

        // ─── Comandos ─────────────────────────────────────────────────────────
        // [RelayCommand] genera automáticamente GetJokeCommand que el XAML puede bindear.
        // El nombre del comando = nombre del método + "Command"

        [RelayCommand]
        private async Task GetJokeAsync()
        {
            IsLoading = true;
            StatusMessage = "Obteniendo chiste...";
            try
            {
                _currentJoke = await _jokeService.GetJokeAsync();
                JokeText = _currentJoke?.Text ?? "No se pudo obtener el chiste.";
                HasJoke = _currentJoke != null;

                // Registrar en historial automáticamente
                if (_currentJoke != null)
                {
                    JokeText = _currentJoke.Text;
                    HasJoke = true;
                    StatusMessage = "Chiste cargado ✓";

                    // Guardar en historial
                    await _historyService.AddAsync("joke", _currentJoke.Id.ToString(), _currentJoke.Text);
                }
                else
                {
                    JokeText = "No se pudo obtener el chiste. Intenta de nuevo.";
                    HasJoke = false;
                    StatusMessage = "Sin respuesta de la API";
                    await _historyService.AddAsync(new HistoryItem
                    {
                        Content = JokeText,
                        Type = "Joke",
                        CreatedAt = DateTime.Now
                    });
                }

                StatusMessage = "Chiste cargado ✓";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener chiste: {ex.Message}";
                JokeText = "Ocurrió un error. Intenta de nuevo.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GetMemeAsync()
        {
            IsLoading = true;
            StatusMessage = "Obteniendo meme...";
            try
            {
                _currentMeme = await _memeService.GetMemeAsync();
                MemeUrl = _currentMeme?.Url ?? string.Empty;
                MemeName = _currentMeme?.Name ?? string.Empty;
                HasMeme = _currentMeme != null;

                // Registrar en historial automáticamente
                if (_currentMeme != null)
                {
                    MemeUrl = _currentMeme.Url;
                    MemeName = _currentMeme.Name;
                    HasMeme = true;
                    StatusMessage = "Meme cargado ✓";
                    // Guardar en historial
                    await _historyService.AddAsync("meme", _currentMeme.Id, _currentMeme.Name);
                }
                else
                {
                    HasMeme = false;
                    StatusMessage = "Sin respuesta de la API";
                    await _historyService.AddAsync(new HistoryItem
                    {
                        Content = MemeUrl,
                        Type = "Meme",
                        CreatedAt = DateTime.Now
                    });
                }

                StatusMessage = "Meme cargado ✓";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener meme: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SaveJokeAsFavoriteAsync()
        {
            if (_currentJoke == null) return;
            try
            {
                await _databaseService.SaveFavoriteAsync(new Favorite
                {
                    Content = JokeText,
                    Type = "Joke",
                    SavedAt = DateTime.Now
                });
                StatusMessage = "Chiste guardado en favoritos ⭐";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar favorito: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SaveMemeAsFavoriteAsync()
        {
            if (_currentMeme == null) return;
            try
            {
                await _databaseService.SaveFavoriteAsync(new Favorite
                {
                    Content = MemeUrl,
                    Type = "Meme",
                    SavedAt = DateTime.Now
                });
                StatusMessage = "Meme guardado en favoritos ⭐";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar favorito: {ex.Message}";
            }
        }

        
        [RelayCommand]
        private void OpenFavorites()
        {
            // TODO: cuando Persona 2 suba su vista
            MessageBox.Show("Vista de Favoritos en construccion.", "JokeHub");
            // TODO: Cuando Persona 2 suba FavoritesView.xaml, reemplaza el MessageBox con:
            // var view = new Views.FavoritesView();
            // view.Show();
            MessageBox.Show("Vista de Favoritos en construcción.", "JokeHub");
        }

        [RelayCommand]
        private void OpenHistory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseSqlite("Data Source=app.db")
           .Options;
            var context = new AppDbContext(options);
            new Views.HistoryView(context).Show();
            // TODO: Cuando Persona 3 suba HistoryView.xaml, reemplaza el MessageBox con:
            // var view = new Views.HistoryView();
            // view.Show();
            MessageBox.Show("Vista de Historial en construcción.", "JokeHub");
        }
    }
}