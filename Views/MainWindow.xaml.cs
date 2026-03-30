using System.Net.Http;
using System.Windows;
using JokeApp.Data;
using JokeApp.Services;
using JokeApp.Services.Interfaces;
using JokeApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JokeApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=app.db")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var httpClient = new HttpClient();
            IJokeService jokeService = new JokeService(httpClient);
            IMemeService memeService = new MemeService(httpClient);
            var historyService = new HistoryService(context);
            var databaseService = new DatabaseService(context);

            databaseService.EnsureFavoritesTableAsync().GetAwaiter().GetResult();

            DataContext = new MainViewModel(jokeService, memeService, historyService, databaseService, context);
        }
    }
}
