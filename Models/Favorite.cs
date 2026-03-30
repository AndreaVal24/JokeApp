using System;

namespace JokeApp.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public string ContentId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public DateTime SavedAt { get; set; }

        public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

        public string DisplayType => ContentType.Equals("meme", StringComparison.OrdinalIgnoreCase)
            ? "meme"
            : "joke";
    }
}
