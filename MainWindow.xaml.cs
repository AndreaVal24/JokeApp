using JokeApp.Services;
using JokeApp.ViewModels;
using System.Windows;

namespace JokeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Aqui se crean los servicios y se conectan al ViewModel.
            // Estos van a mostrar errores hasta que tus compañeros suban sus archivos.
            // Eso es normal — no te preocupes por eso ahora.
            var jokeService = new JokeService();
            var memeService = new MemeService();
            var historyService = new HistoryService();
            var databaseService = new DatabaseService();

            DataContext = new MainViewModel(jokeService, memeService, historyService, databaseService);
        }
    }
}