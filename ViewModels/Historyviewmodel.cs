using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JokeApp.Models;
using JokeApp.Services;

namespace JokeApp.ViewModels
{
    /// <summary>
    /// ViewModel de la vista de historial. Sigue el patrón MVVM y hereda de
    /// <see cref="ObservableObject"/> (CommunityToolkit.Mvvm) para notificar
    /// automáticamente a la UI cuando cambian las propiedades.
    /// </summary>
    /// <remarks>
    /// La clase usa el generador de código de CommunityToolkit:
    /// los campos con <c>[ObservableProperty]</c> generan automáticamente
    /// su propiedad pública con <c>INotifyPropertyChanged</c>.
    /// Los métodos con <c>[RelayCommand]</c> generan su comando asociado.
    /// </remarks>
    public partial class HistoryViewModel : ObservableObject
    {

        // ─── Dependencias ──────────────────────────────────────────────────────

        /// <summary>
        /// Servicio que gestiona las operaciones de historial en la BD.
        /// </summary>
        private readonly HistoryService _historyService;

        // ─── Propiedades observables ───────────────────────────────────────────

        /// <summary>
        /// Colección observable de registros del historial.
        /// Al ser <see cref="ObservableCollection{T}"/>, la UI se actualiza
        /// automáticamente cuando se agregan o eliminan elementos.
        /// Bindeada al ListView en <c>HistoryView.xaml</c>.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<HistoryItem> _historyItems = new();

        /// <summary>
        /// Controla la visibilidad del indicador de carga.
        /// Se activa mientras se consulta la base de datos.
        /// </summary>
        [ObservableProperty]
        private bool _isLoading = false;

        /// <summary>
        /// Mensaje de estado mostrado en la parte inferior de la vista.
        /// Informa al usuario sobre el resultado de las operaciones.
        /// </summary>
        [ObservableProperty]
        private string _statusMessage = "Historial listo";

        /// <summary>
        /// Indica si el historial tiene al menos un registro.
        /// Controla si el botón "Limpiar" está habilitado en la UI.
        /// </summary>
        [ObservableProperty]
        private bool _hasItems = false;

        // ─── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Inicializa el ViewModel con el servicio de historial
        /// y carga los datos inmediatamente al abrir la vista.
        /// </summary>
        /// <param name="historyService">
        /// Servicio inyectado manualmente desde <c>HistoryView.xaml.cs</c>.
        /// </param>
        public HistoryViewModel(HistoryService historyService)
        {
            _historyService = historyService;

            // Carga el historial al instanciar el ViewModel
            // Se usa Task.Run para no bloquear el hilo de UI
            _ = LoadHistoryAsync();
        }

        // ─── Comandos ──────────────────────────────────────────────────────────

        /// <summary>
        /// Carga (o recarga) el historial desde la base de datos.
        /// Actualiza <see cref="HistoryItems"/> con los datos más recientes,
        /// ordenados del más nuevo al más antiguo.
        /// </summary>
        [RelayCommand]
        private async Task LoadHistoryAsync()
        {
            IsLoading = true;
            StatusMessage = "Cargando historial...";

            try
            {
                var items = await _historyService.GetAllAsync();

                HistoryItems.Clear();
                foreach (var item in items)
                    HistoryItems.Add(item);

                HasItems = HistoryItems.Count > 0;
                StatusMessage = HistoryItems.Count > 0
                    ? $"{HistoryItems.Count} registro(s) en el historial"
                    : "El historial está vacío";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar historial: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Elimina todos los registros del historial de la base de datos
        /// y limpia la colección visible en pantalla.
        /// </summary>
        [RelayCommand]
        private async Task ClearHistoryAsync()
        {
            if (!HasItems) return;

            IsLoading = true;
            StatusMessage = "Limpiando historial...";

            try
            {
                await _historyService.ClearAllAsync();
                HistoryItems.Clear();
                HasItems = false;
                StatusMessage = "Historial limpiado ✓";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al limpiar historial: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
