using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public int TmdbId { get; set; } // TMDB'den gelecek
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; } // Afiş URL
        public string? BackdropPath { get; set; } // Arka plan
        public DateTime? ReleaseDate { get; set; }
        public double? VoteAverage { get; set; } // IMDB puanı gibi
        public int? Runtime { get; set; } // Dakika
        public DateTime WatchedDate { get; set; } // Ne zaman izlendi
        public int? UserRating { get; set; } // Kullanıcının verdiği puan (1-10)
        public string? UserNotes { get; set; } // Kullanıcı notu
        public string? Genres { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<MovieLike> Likes { get; set; } = new List<MovieLike>();
    }
}
