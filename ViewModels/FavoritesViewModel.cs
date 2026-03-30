using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JokeApp.Models;
using JokeApp.Services;

namespace JokeApp.ViewModels
{
    public partial class FavoritesViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Favorite> _favorites = new();

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = "Favoritos listos";

        [ObservableProperty]
        private bool _hasItems = false;

        public FavoritesViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _ = LoadFavoritesAsync();
        }

        [RelayCommand]
        private async Task LoadFavoritesAsync()
        {
            IsLoading = true;
            StatusMessage = "Cargando favoritos...";

            try
            {
                var items = await _databaseService.GetAllFavoritesAsync();

                Favorites.Clear();
                foreach (var item in items)
                {
                    Favorites.Add(item);
                }

                HasItems = Favorites.Count > 0;
                StatusMessage = HasItems
                    ? $"{Favorites.Count} favorito(s) guardado(s)"
                    : "No hay favoritos guardados";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar favoritos: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task DeleteFavoriteAsync(Favorite? favorite)
        {
            if (favorite == null)
            {
                return;
            }

            IsLoading = true;
            StatusMessage = "Eliminando favorito...";

            try
            {
                var deleted = await _databaseService.DeleteFavoriteAsync(favorite.Id);
                if (deleted)
                {
                    Favorites.Remove(favorite);
                    HasItems = Favorites.Count > 0;
                    StatusMessage = HasItems
                        ? $"{Favorites.Count} favorito(s) guardado(s)"
                        : "No hay favoritos guardados";
                }
                else
                {
                    StatusMessage = "No se encontro el favorito";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar favorito: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
