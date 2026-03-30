using System.Windows;
using JokeApp.Data;
using JokeApp.Services;
using JokeApp.ViewModels;

namespace JokeApp.Views
{
    public partial class FavoritesView : Window
    {
        public FavoritesView(AppDbContext context)
        {
            InitializeComponent();

            var databaseService = new DatabaseService(context);
            DataContext = new FavoritesViewModel(databaseService);
        }
    }
}
