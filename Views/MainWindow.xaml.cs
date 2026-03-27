using System.Net.Http;
using System.Windows;
using HumorApp.Services;
using JokeApp.Services;
using JokeApp.ViewModels;

namespace JokeApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            var jokeService = new JokeService(httpClient);
            var memeService = new MemeService(httpClient);

            DataContext = new MainViewModel(jokeService, memeService);
        }
    }
}