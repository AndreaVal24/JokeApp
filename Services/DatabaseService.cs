using JokeApp.Data;
using JokeApp.Models;
using JokeApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JokeApp.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task EnsureFavoritesTableAsync()
        {
            const string createTableSql = @"
CREATE TABLE IF NOT EXISTS Favorites (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ContentType TEXT NOT NULL,
    ContentId TEXT NOT NULL,
    Title TEXT NOT NULL,
    Content TEXT NULL,
    ImageUrl TEXT NULL,
    SavedAt TEXT NOT NULL
);";

            const string createUniqueIndexSql = @"
CREATE UNIQUE INDEX IF NOT EXISTS IX_Favorites_ContentType_ContentId
ON Favorites (ContentType, ContentId);";

            const string createSavedAtIndexSql = @"
CREATE INDEX IF NOT EXISTS IX_Favorites_SavedAt
ON Favorites (SavedAt);";

            await _context.Database.ExecuteSqlRawAsync(createTableSql);
            await _context.Database.ExecuteSqlRawAsync(createUniqueIndexSql);
            await _context.Database.ExecuteSqlRawAsync(createSavedAtIndexSql);
        }

        public async Task<List<Favorite>> GetAllFavoritesAsync()
        {
            return await _context.Favorites
                .OrderByDescending(f => f.SavedAt)
                .ToListAsync();
        }

        public async Task<bool> SaveJokeAsync(JokeDto joke)
        {
            var favorite = new Favorite
            {
                ContentType = "joke",
                ContentId = joke.Id.ToString(),
                Title = BuildJokeTitle(joke.Text),
                Content = joke.Text,
                ImageUrl = null,
                SavedAt = DateTime.Now
            };

            return await UpsertFavoriteAsync(favorite);
        }

        public async Task<bool> SaveMemeAsync(MemeDto meme)
        {
            var favorite = new Favorite
            {
                ContentType = "meme",
                ContentId = meme.Id,
                Title = meme.Name,
                Content = string.Empty,
                ImageUrl = meme.Url,
                SavedAt = DateTime.Now
            };

            return await UpsertFavoriteAsync(favorite);
        }

        public async Task<bool> DeleteFavoriteAsync(int id)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.Id == id);
            if (favorite == null)
            {
                return false;
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UpsertFavoriteAsync(Favorite favorite)
        {
            var existing = await _context.Favorites.FirstOrDefaultAsync(f =>
                f.ContentType == favorite.ContentType &&
                f.ContentId == favorite.ContentId);

            if (existing == null)
            {
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
                return true;
            }

            existing.Title = favorite.Title;
            existing.Content = favorite.Content;
            existing.ImageUrl = favorite.ImageUrl;
            existing.SavedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return false;
        }

        private static string BuildJokeTitle(string text)
        {
            var firstLine = text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? string.Empty;

            var title = firstLine.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Chiste";
            }

            return title.Length <= 60 ? title : $"{title[..57]}...";
        }
    }
}
