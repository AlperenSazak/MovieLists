using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TmdbMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Original_Title { get; set; }
        public string? Overview { get; set; }
        public string? Poster_Path { get; set; }
        public string? Backdrop_Path { get; set; }
        public string? Release_Date { get; set; }
        public double Vote_Average { get; set; }
        public int? Runtime { get; set; }
        public string? Tagline { get; set; } 

        public string GetFullPosterPath()
        {
            return string.IsNullOrEmpty(Poster_Path)
                ? null
                : $"https://image.tmdb.org/t/p/w500{Poster_Path}";
        }

        public string GetFullBackdropPath()
        {
            return string.IsNullOrEmpty(Backdrop_Path)
                ? null
                : $"https://image.tmdb.org/t/p/original{Backdrop_Path}";
        }

        public List<Genre> Genres { get; set; }

        public TmdbVideosDto Videos { get; set; }

        public class Genre
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }

    public class TmdbVideosDto
    {
        [JsonPropertyName("results")]
        public List<TmdbVideoDto> Results { get; set; }
    }

    public class TmdbVideoDto
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}