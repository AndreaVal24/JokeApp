using System.Net.Http;
using System.Windows;
using HumorApp.Services;
using JokeApp.Data;
using JokeApp.Services;
using JokeApp.Services.Interfaces;
using JokeApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JokeApp
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
            var memeService = new MemeService(httpClient);
            var historyService = new HistoryService(context);

            DataContext = new MainViewModel(jokeService, memeService, historyService);
        }
    }
}