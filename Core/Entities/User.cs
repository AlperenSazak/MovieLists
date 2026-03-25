using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int? FavoriteTmdbId { get; set; }
        public string? ProfilePhoto { get; set; }

        // Navigation Property
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<MovieLike> Likes { get; set; } = new List<MovieLike>();
        public ICollection<WatchLater> WatchLaterMovies { get; set; } = new List<WatchLater>();
    }
}
