using System;
using System.Windows;
using JokeApp.Data;
using JokeApp.Services;
using JokeApp.ViewModels;

namespace JokeApp.Views
{
    /// <summary>
    /// Code-behind de la vista de historial.
    /// Siguiendo el patrón MVVM, este archivo solo se encarga de:
    /// 1. Inicializar los componentes visuales (InitializeComponent).
    /// 2. Instanciar el ViewModel con sus dependencias.
    /// 3. Asignar el DataContext para que el binding funcione.
    /// Ninguna lógica de negocio debe vivir aquí.
    /// </summary>
    public partial class HistoryView : Window
    {
        /// <summary>
        /// Constructor de la vista. Instancia el servicio y el ViewModel
        /// manualmente, siguiendo el mismo patrón que usa <c>MainWindow.xaml.cs</c>.
        /// </summary>
        /// <param name="context">
        /// Contexto de base de datos recibido desde <c>MainWindow</c>
        /// para reutilizar la misma instancia de conexión.
        /// </param>
        public HistoryView(AppDbContext context)
        {
            InitializeComponent();

            var historyService = new HistoryService(context);
            DataContext = new HistoryViewModel(historyService);
        }
    }
}